using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ArithmeticCoding
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void confirmButton_Click(object sender, EventArgs e)
        {
            Tuple<long[], char[]> lettersAndQuantity = calculateRepetitionForEachLetter();
            char[] lettersWithoutRepeat = lettersAndQuantity.Item2;
            long[] quantityForEachLetter = lettersAndQuantity.Item1;
            double segmentOfMessage = calculateSegmentOfMessage(quantityForEachLetter);
            MessageBox.Show(segmentOfMessage.ToString());
        }

        private Tuple<long[], char[]> calculateRepetitionForEachLetter()
        {
            string message = stringTextBox.Text;
            List<char> lettersWithoutRepeat = new List<char>();
            var letterQuantity = new List<long>();


            foreach (char letterFromMessage in message)
            {

                bool isLetterInList = false;

                for (int i = 0; i < lettersWithoutRepeat.Count; i++)
                {
                    if (lettersWithoutRepeat[i] == letterFromMessage)
                    {
                        isLetterInList = true;
                        letterQuantity[i] += 1;
                    }
                }

                if (!isLetterInList)
                {
                    lettersWithoutRepeat.Add(letterFromMessage);
                    letterQuantity.Add(1);
                }
            }
            long[] letterQuantityArray = letterQuantity.ToArray();
            char[] lettersWithoutRepeatArray = lettersWithoutRepeat.ToArray();
            Array.Sort(letterQuantityArray,lettersWithoutRepeatArray);
            Array.Reverse(letterQuantityArray);
            Array.Reverse(lettersWithoutRepeatArray);

            return Tuple.Create(letterQuantityArray, lettersWithoutRepeatArray);

        }

        private double calculateSegmentOfMessage(long[] quantityForEachLetter)
        {
            double segmentSize = 0;
            foreach (long quantity in quantityForEachLetter)
            {
                segmentSize += Convert.ToDouble(quantity);
            }

            return 1 / segmentSize;
        }
    }
}
