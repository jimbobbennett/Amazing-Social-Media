using System;
using System.Threading.Tasks;
using Microsoft.Azure.CognitiveServices.ContentModerator;
using Microsoft.CognitiveServices.ContentModerator;
using Plugin.Media.Abstractions;
using Microsoft.Azure.CognitiveServices.Vision.Face;
using Microsoft.Azure.CognitiveServices.Vision.Face.Models;
using System.Linq;

namespace AmazingSocialMedia.Services
{
    public class ContentModeratorService
    {
        private ContentModeratorClient _client;
        private FaceAPI _faceApi;

        private void InitIfRequired()
        {
            if (_client == null)
            {
                var credentials = new ApiKeyServiceClientCredentials(ApiKeys.ContentModeratorKey);
                _client = new ContentModeratorClient(credentials);
                _client.BaseUrl = ApiKeys.ContentModeratorBaseUrl;
            }

            if (_faceApi == null)
            {
                var credentials = new ApiKeyServiceClientCredentials(ApiKeys.FaceApiKey);
                _faceApi = new FaceAPI(credentials);
                _faceApi.AzureRegion = ApiKeys.FaceApiRegion;
            }
        }

        public async Task<bool> IsFace(MediaFile photo)
        {
            InitIfRequired();

            using (var stream = photo.GetStreamWithImageRotatedForExternalStorage())
            {
                var result = await new FaceOperations(_faceApi).DetectInStreamAsync(stream);
                return result != null && result.Any();
            }
        }

        public async Task<bool> IsDuckFace(MediaFile photo)
        {
            return false;
        }

        public async Task<bool> ContainsProfanity(string text)
        {
            InitIfRequired();

            var lang = await _client.TextModeration.DetectLanguageAsync("text/plain", text);
            var moderation = await _client.TextModeration.ScreenTextAsync(lang.DetectedLanguageProperty, "text/plain", text);
            return moderation.Terms == null || !moderation.Terms.Any();
        }
    }
}
