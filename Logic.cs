using System;
using System.Collections.Generic;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Metro
{
    struct for_help{
        public int position;
        public int mode;
    }

    class Logic
    {
        public const int ROW_MODE = 0, COLUMN_MODE = 1;
        public const int LEFT = 0, TOP = 1, RIGHT = 2, BOTTOM = 3;
        public const int HORIZONTALLY = 0, VERTICALLY = 1;

        private Random random;
        private char[] fName, lName;
        private Dictionary<char, Bitmap> mapImage;
        private Dictionary<int, string> nameImage;

        private for_help fNameH, lNameH;

        public Logic(String[] str,  Dictionary<char, Bitmap> mapImage, Dictionary<int, string> nameImage)
        {
            random = new Random();

            fName = str[0].ToUpper().ToCharArray();
            lName = str[1].ToUpper().ToCharArray();
            this.mapImage = mapImage;
            this.nameImage = nameImage;
        }

        public void rearrangeLName()
        {
            int lNameLen = lName.Length;

            int diffLength = Form1.sizeTable - lNameLen + 1;

            int mode = random.Next(2);
            int biasRow, biasCol, begin, end;

            switch (mode)
            {
                case ROW_MODE:
                    biasRow = random.Next(Form1.sizeTable);
                    biasCol = random.Next(diffLength);

                    begin = biasRow * Form1.sizeTable + biasCol;
                    end = begin + lNameLen;

                    draw(lName, begin, end, 1);

                    rearrangeFName(biasRow, biasCol, mode);
                    break;
                case COLUMN_MODE:
                    biasRow = random.Next(diffLength);
                    biasCol = random.Next(Form1.sizeTable);

                    begin = biasRow * Form1.sizeTable + biasCol;
                    end = begin + Form1.sizeTable * lNameLen;

                    draw(lName, begin, end, Form1.sizeTable);

                    rearrangeFName(biasRow, biasCol, mode);

                    break;
            }
        }

        private void rearrangeFName(int numbRow, int numbColumn, int mode)
        {
            List<int> coincidenceLName = new List<int>();
            List<int> coincidenceFName = new List<int>();


            int lNameLen = lName.Length;
            int fNameLen = fName.Length;

            int begin, end;

            for (int i = 0; i < lNameLen; i++)
            {
                for (int j = 0; j < fNameLen; j++)
                {
                    if (lName[i] == fName[j])
                    {
                        coincidenceLName.Add(i);
                        coincidenceFName.Add(j);
                    }
                }
            }

            int tmp_mode = random.Next(4);
            bool failed_tmp_mode = false;
            int sizeCC;

            switch (mode)
            {
                case ROW_MODE:
                fMode:
                    if (failed_tmp_mode || coincidenceLName.Count == 0 || tmp_mode == 0)
                    {
                        rowModeWithoutCoinc(numbRow, fNameLen);
                    }
                    else
                    {
                        int sizeListCoincid = coincidenceFName.Count;
                        int sizeFPart, sizeSecPart;
                        List<int> correctCoincid = new List<int>();

                        for (int i = 0; i < sizeListCoincid; i++)
                        {
                            sizeFPart = coincidenceFName[i];
                            sizeSecPart = fNameLen - sizeFPart;

                            int diffRow = Form1.sizeTable - numbRow;
                            if (numbRow > sizeFPart && diffRow > sizeSecPart)
                            {
                                correctCoincid.Add(i);
                            }
                            
                        }

                        sizeCC = correctCoincid.Count;
                        if (sizeCC == 0)
                        {
                            failed_tmp_mode = true;
                            goto fMode;
                        }

                        int ind = correctCoincid[random.Next(sizeCC)];

                        sizeFPart = coincidenceFName[ind];
                        sizeSecPart = fNameLen - sizeFPart;

                        begin = (numbRow - sizeFPart) * Form1.sizeTable + numbColumn + coincidenceLName[ind];
                        end = begin + Form1.sizeTable * fNameLen;

                        draw(fName, begin, end, Form1.sizeTable);
                    }
                    break;
                case COLUMN_MODE:   
                fMode2:
                    if (failed_tmp_mode || coincidenceLName.Count == 0 || tmp_mode == 0)
                    {
                        columnModeWithoutCoinc(numbColumn, fNameLen);
                    }
                    else
                    {
                        int sizeListCoincid = coincidenceFName.Count;
                        int sizeFPart, sizeSecPart;
                        List<int> correctCoincid = new List<int>();

                        for (int i = 0; i < sizeListCoincid; i++)
                        {
                            sizeFPart = coincidenceFName[i];
                            sizeSecPart = fNameLen - sizeFPart;

                            int diffRow = Form1.sizeTable - numbColumn;
                            if (numbColumn > sizeFPart && diffRow > sizeSecPart)
                            {
                                correctCoincid.Add(i);
                            }
                        }

                        sizeCC = correctCoincid.Count;

                        if (sizeCC == 0)
                        {
                            failed_tmp_mode = true;
                            goto fMode2;
                        }

                        int ind = correctCoincid[random.Next(sizeCC)];

                        sizeFPart = coincidenceFName[ind];
                        sizeSecPart = fNameLen - sizeFPart;

                        begin = (coincidenceLName[ind] + numbRow) * Form1.sizeTable + numbColumn - sizeFPart;
                        end = begin + fNameLen;

                        draw(fName, begin, end, 1);
                    }
                    break;
            }
        }

        private void rowModeWithoutCoinc(int numbRow, int fNameLen)
        {
            int tmp_row = 0;
            int begin, end;

            do
            {
                tmp_row = random.Next(Form1.sizeTable);
            } while (tmp_row == numbRow);

            int biasCol = random.Next(Form1.sizeTable - fNameLen);

            begin = tmp_row * Form1.sizeTable + biasCol;
            end = begin + fNameLen;

            draw(fName, begin, end, 1);
        }

        private void columnModeWithoutCoinc(int numbColumn, int fNameLen)
        {
            int tmp_col = 0;
            int begin, end;

            do
            {
                tmp_col = random.Next(Form1.sizeTable);
            } while (tmp_col == numbColumn);

            int biasRow = random.Next(Form1.sizeTable - fNameLen);

            begin = biasRow * Form1.sizeTable + tmp_col;
            end = begin + Form1.sizeTable * fNameLen;
            draw(fName, begin, end, Form1.sizeTable);
        }

        private void draw(char[] text, int begin, int end, int step)
        {
            int numb = 0;
            if (text == fName)
            {
                fNameH = new for_help();
                fNameH.position = begin;
                fNameH.mode = (step == 1 ? ROW_MODE : COLUMN_MODE);
            }
            else
            {
                lNameH = new for_help();
                lNameH.position = begin;
                lNameH.mode = (step == 1 ? ROW_MODE : COLUMN_MODE);
            }

            for (int i = begin; i < end; i+= step)
            {
                char key = text[numb];
                numb++;

                Form1.listLabel[i].Image = mapImage[key];

                nameImage.Remove(i);
                string nameImg = "" + key;
                nameImage.Add(i, nameImg);
            }
        }

        public for_help getFName()
        {
            return fNameH;
        }

        public for_help getLName()
        {
            return lNameH;
        }
    }
}