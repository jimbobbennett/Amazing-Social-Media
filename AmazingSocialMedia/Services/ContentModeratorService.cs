using System.Linq;
using System.Threading.Tasks;
using Microsoft.Azure.CognitiveServices.ContentModerator;
using Microsoft.Azure.CognitiveServices.Vision.Face;
using Microsoft.Cognitive.CustomVision.Prediction;
using Microsoft.CognitiveServices.ContentModerator;
using Plugin.Media.Abstractions;

namespace AmazingSocialMedia.Services
{
    public class ContentModeratorService
    {
		private const double ProbabilityThreshold = 0.5;
        private PredictionEndpoint _endpoint;
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

            if (_endpoint == null)
                _endpoint = new PredictionEndpoint { ApiKey = ApiKeys.PredictionKey };
        }

        public async Task<bool> IsFace(MediaFile photo)
        {
            InitIfRequired();
            if (photo == null) return false;

            using (var stream = photo.GetStreamWithImageRotatedForExternalStorage())
            {
                var result = await new FaceOperations(_faceApi).DetectInStreamAsync(stream);
                return result != null && result.Any();
            }
        }

        public async Task<bool> IsDuckFace(MediaFile photo)
        {
            InitIfRequired();
            if (photo == null) return false;

            using (var stream = photo.GetStreamWithImageRotatedForExternalStorage())
            {
                var predictionModels = await _endpoint.PredictImageAsync(ApiKeys.ProjectId, stream);
                return predictionModels.Predictions
                                       .FirstOrDefault(p => p.Tag == "Duck Face")
                                       .Probability > ProbabilityThreshold;
            }
        }

        public async Task<bool> ContainsProfanity(string text)
        {
            InitIfRequired();
            if (string.IsNullOrEmpty(text)) return false;

            var lang = await _client.TextModeration.DetectLanguageAsync("text/plain", text);
            var moderation = await _client.TextModeration.ScreenTextAsync(lang.DetectedLanguageProperty, "text/plain", text);
            return moderation.Terms != null && moderation.Terms.Any();
        }
    }
}
