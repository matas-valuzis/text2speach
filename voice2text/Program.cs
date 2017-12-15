using System;
using System.Linq;
using System.Speech.Recognition;
using System.Threading.Tasks;


namespace voice2text
{
    class Program
    {
        static void Main(string[] args)
        {
            var numberChoices = new Choices();

            for (int i = 0; i <= 100; i++)
            {
                numberChoices.Add(i.ToString());
            }
            var gb = new GrammarBuilder();
            gb.Append("Select player");
            gb.Append(new SemanticResultKey("number", numberChoices));
            
            var sr = new SpeechRecognitionEngine();
            sr.SetInputToDefaultAudioDevice();
            sr.LoadGrammar(new Grammar(gb));
            sr.SpeechRecognized += SpeechRecognized;
            sr.SpeechDetected += SpeechDetected;
            sr.SpeechRecognitionRejected += SpeechRejected;
            // loop recognition
            sr.RecognizeCompleted += (s, e) => sr.RecognizeAsync();

            Console.WriteLine("Say phrase \"Select player {0-100}\"");

            sr.RecognizeAsync();
            Console.Read();
            sr.RecognizeAsyncCancel();
            
        }

        private static void SpeechRejected(object sender, SpeechRecognitionRejectedEventArgs e)
        {
            Console.WriteLine("Command not recognized");
        }

        private static void SpeechDetected(object sender, SpeechDetectedEventArgs e)
        {
            Console.WriteLine("...");
        }

        static void SpeechRecognized(object sender, SpeechRecognizedEventArgs e)
        {
            var res = e.Result.Semantics["number"].Value; 
            Console.WriteLine("Selected player #{0}", res);
            
        }

 

    }
}
