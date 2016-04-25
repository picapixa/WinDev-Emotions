using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media;

/// <summary>
/// Want a quicker way to define your model from a JSON-based API? Go to its documentation
/// and copy the sample JSON data with the format you would like to use.
/// 
/// In this case, from the API Reference: https://dev.projectoxford.ai/docs/services/5639d931ca73072154c1ce89/operations/563b31ea778daf121cc3a5fa
/// - Find the sample output from the success code, and copy it.
/// - Here in Visual Studio, go to Edit->Paste Special->Paste JSON as classes.
/// - Replace the name of the generated Class1 to EmotionData.
/// - Remove the class with the array definition, as the collection of emotion data
///   will be set at the view model and not here.
/// Voila! 
/// 
/// In the spirit of following naming conventions, you may change the property names
/// to CamelCase, ex: faceRectangle -> FaceRectangle. 
/// </summary>
namespace WinDev_Emotions.Model
{

    public class EmotionData
    {
        public FaceRectangle FaceRectangle { get; set; }
        public Scores Scores { get; set; }

        /// <summary>
        /// This property is not part of the output of the API but will serve as the
        /// reference for drawing the rectangles in the view. Moving the data from the
        /// FaceRetangle class to here will be processed at the view model.
        /// </summary>
        public RectangleGeometry RectGeometry { get; set; }
    }

    public class FaceRectangle
    {
        public int Left { get; set; }
        public int Top { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
    }

    public class Scores
    {
        public float Anger { get; set; }
        public float Contempt { get; set; }
        public float Disgust { get; set; }
        public float Fear { get; set; }
        public float Happiness { get; set; }
        public float Neutral { get; set; }
        public float Sadness { get; set; }
        public float Surprise { get; set; }
    }

}
