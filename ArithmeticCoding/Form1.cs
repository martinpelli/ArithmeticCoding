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
            Tuple<List<long>, List<char>> lettersAndQuantityList = calculateRepetitionForEachLetter();
            List<char> lettersWithoutRepeatList = lettersAndQuantityList.Item2;
            List<long> quantityForEachLetterList = lettersAndQuantityList.Item1;

            Tuple<long[], char[]> lettersAndQuantity = orderQuantityAndLetters(quantityForEachLetterList,lettersWithoutRepeatList);
            
            Tuple<long[], char[]> lettersAndQuantityOrdered = orderAlphabeticallySameQuantityLetters(lettersAndQuantity.Item1, lettersAndQuantity.Item2, 0);
            char[] lettersWithoutRepeatArray = lettersAndQuantityOrdered.Item2;
            long[] quantityForEachLetterArray = lettersAndQuantityOrdered.Item1;

            Tuple<double, double> lowerAndUpperBounds = calculateLowerAndUpperBounds(quantityForEachLetterArray,lettersWithoutRepeatArray);
            double upperBound = lowerAndUpperBounds.Item2;
            double lowerBound = lowerAndUpperBounds.Item1;

        }

        private Tuple<List<long>, List<char>> calculateRepetitionForEachLetter()
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

            return Tuple.Create(letterQuantity, lettersWithoutRepeat);

        }

        private Tuple<long[], char[]> orderQuantityAndLetters(List<long> letterQuantity, List<char> lettersWithoutRepeat)
        {
            long[] letterQuantityArray = letterQuantity.ToArray();
            char[] lettersWithoutRepeatArray = lettersWithoutRepeat.ToArray();
            Array.Sort(letterQuantityArray, lettersWithoutRepeatArray);
            Array.Reverse(letterQuantityArray);
            Array.Reverse(lettersWithoutRepeatArray);

            return Tuple.Create(letterQuantityArray, lettersWithoutRepeatArray);
        }

        private Tuple<long[], char[]> orderAlphabeticallySameQuantityLetters(long[] letterQuantity, char[] lettersWithoutRepeat,int from)
        {
            bool isFirst = false;
            int to = 1;
            int length = 1;

            
            for(int i = from; i < lettersWithoutRepeat.Length; i++)
            {
                if ((i+1 <lettersWithoutRepeat.Length) && letterQuantity[i] == letterQuantity[i + 1])
                {
                    if (!isFirst)
                    {
                        isFirst = true;
                        from = i;
                        to = from;
                    }
                    to += 1;
                    length += 1;
                    continue;
                }
                else
                {
                    if (isFirst)
                    {
                        break;
                    }
                }
                
            }
            if (isFirst)
            {
                Array.Sort(lettersWithoutRepeat, from, length);
                return orderAlphabeticallySameQuantityLetters(letterQuantity, lettersWithoutRepeat, to);
            }
            else
            {
                return Tuple.Create(letterQuantity, lettersWithoutRepeat);
            }
            
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
                        MessageBox.Show("Cota Inferior: " + lowerBound.ToString());
                        MessageBox.Show("Cota Superior: " + upperBound.ToString());
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
