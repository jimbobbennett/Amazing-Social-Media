using System;
using Microsoft.Azure.CognitiveServices.Vision.Face.Models;

namespace AmazingSocialMedia
{
    public static class ApiKeys
    {
#error You need to set up your API keys.
        // Start by registering for an account at https://aka.ms/Hi27dz
        // Then create a new project.
        // From the settings tab, find:
        // Prediction Key
        // Project Id
        // and update the values below
        public static string PredictionKey = "<Your Prediction Key>";
        public static Guid ProjectId = Guid.Parse("<Your Project GUID>");
    
        // To get a content moderator key, sign up at https://aka.ms/Xnyvh9
        // Once signed up you will see the Azure region
        public const string ContentModeratorKey = "";
        public const string ContentModeratorBaseUrl = "";

        // To get a FaceAPI key, sign up at https://aka.ms/K5qesz
        // Once signed up you will see the Azure region for your service
        public const string FaceApiKey = "";
        public const AzureRegions FaceApiRegion = AzureRegions.Westcentralus; 
    }
}
