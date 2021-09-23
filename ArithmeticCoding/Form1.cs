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
            Tuple<double, double> lowerAndUpperBounds = calculateLowerAndUpperBounds(quantityForEachLetter,lettersWithoutRepeat);
            double upperBound = lowerAndUpperBounds.Item2;
            double lowerBound = lowerAndUpperBounds.Item1;

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

        private Tuple<double, double> calculateLowerAndUpperBounds(long[] quantityForEachLetter,char[] lettersWithoutRepeat)
        {
            string message = stringTextBox.Text;
            double messageSize = Convert.ToDouble(message.Length);
            double upperBound = 1;
            double lowerBound = 0;
            double segmentOfMessage = calculateSegmentOfMessage(upperBound, messageSize);
            for (int i = 0; i < message.Length; i++)
            {

                int positionOfLetter = 0;
                long numberOfLettersBehind = 0;

                for (int j = lettersWithoutRepeat.Length -1; j > -1; j--)
                {
                    if (message[i] == lettersWithoutRepeat[j])
                    {
                        //MessageBox.Show("Leyendo la letra: " + message[i].ToString());
                        //MessageBox.Show("Tiene esta cant de letras atras: " + numberOfLettersBehind.ToString());
                        upperBound = lowerBound + ((numberOfLettersBehind + quantityForEachLetter[j]) * segmentOfMessage);
                        lowerBound += numberOfLettersBehind * segmentOfMessage;
                        //MessageBox.Show("Cota Inferior: " + lowerBound.ToString());
                        //MessageBox.Show("Cota Superior: " + upperBound.ToString());
                        numberOfLettersBehind = 0;
                        positionOfLetter = 0;
                        break;
                    }
                    else
                    {
                        positionOfLetter += 1;
                        numberOfLettersBehind += quantityForEachLetter[j];
                    }
                }
                segmentOfMessage = calculateSegmentOfMessage(upperBound - lowerBound, messageSize);

            }

            return Tuple.Create(lowerBound,upperBound);
        }

        private double calculateSegmentOfMessage(double range, double messageSize)
        {
            return range / messageSize;
        }


    }
}
