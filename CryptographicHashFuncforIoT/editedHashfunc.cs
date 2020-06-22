using System;
using System.IO;
using System.Linq;
using System.Text;

namespace MyHashFunction
{
    
    class Program
    {

        public static void createNewHashArray() {

            Char[,] thehasharrays = new char[64, 16];
            string text = "";

            // /Users/Ayas/Desktop/hasharrays.txt
            Random rand = new Random();

            for (int i = 0; i < thehasharrays.GetLength(0); i++)
            {
                for (int j = 0; j < thehasharrays.GetLength(1); j++)
                {
                    int num = rand.Next(2);
                    text = text + num.ToString();
                }
                text = text + "\n";
            }

            var path = @"/Users/Ayas/Desktop/hasharray2.txt";


            File.WriteAllText(path, text);

            Console.WriteLine("text written");
        }

        public static Char[,] readHashArray(string path, int size) {

            Char[,] thehasharrays = new char[64, size];

            string[] lines = File.ReadAllLines(path);

            Char[] temp = new char[size];
            for (int i = 0; i < thehasharrays.GetLength(0); i++)
            {
                temp = lines[i].ToCharArray();

                for (int j = 0; j < thehasharrays.GetLength(1); j++)
                {
                    thehasharrays[i, j] = temp[j];
                }
            }

            return thehasharrays;
        }

        public static byte[] ConvertToByteArray(string str, Encoding encoding)
        {
            return encoding.GetBytes(str);
        }

        public static String ToBinary(Byte[] data)
        {
            return string.Join("", data.Select(byt => Convert.ToString(byt, 2).PadLeft(8, '0')));
        }

        public static int ByteCount(string theData) {

            int bytecount = System.Text.ASCIIEncoding.ASCII.GetByteCount(theData);

            return bytecount;
        }

        public static int BitCount(string theData)
        {

            int bitcount = 8 * ByteCount(theData); ;

            return bitcount;
        }

        public static string thesecondHashfunction(string input)
        {
            //it get 8192 byte 0111101010011.. file, so we divide 128 bit parts 
            //65536 bit
            //it equals 512 parts.
            //her row 128 bit 16 byte, o halde row sayısı 512. 
            int asd = input.Length; // 8192 byte 128bits = 16 bytes, 4 byte  for each 32
            //get 8192 character it's bit for me, so 1024bytes
            int adsa = BitCount(input);
            int bitrange = 128;
            int subbitrange = 32;
            int rows = input.Length / bitrange;
            string[,] arr = new string[rows, 4];
            
            for (int i = 0, place = 0; i < rows; i++)
            {
                arr[i, 0] = input.Substring(place, 32); place += 32; //a needs to be 64bit 4byte, 1char 2byte
                arr[i, 1] = input.Substring(place, 32); place += 32; //b
                arr[i, 2] = input.Substring(place, 32); place += 32; //t
                arr[i, 3] = input.Substring(place, 32); place += 32; //g
            }

            string[] resulstr = new string[4];
            for (int j = 0; j < rows - 1 ; j++)
            {
                for (int i = 0; i < 4; i++)
                {
                    //11000101110101101100010111010110
                    //10110110101111011011011010111101
                    for (int k = 0; k < 32; k++) //64 satır hepsi 16, 4 tane 4 lü sahibi
                    {
                        if (arr[j, i].Substring(k, 1) == arr[(j + 1), i].Substring(k, 1))
                        {
                            resulstr[i] += "1";
                        }
                        else
                        {
                            resulstr[i] += "0";
                        }
                    }

                }

                arr[(j + 1), 0] = resulstr[0];
                arr[(j + 1), 1] = resulstr[1];
                arr[(j + 1), 2] = resulstr[2];
                arr[(j + 1), 3] = resulstr[3];

                resulstr[0] = ""; resulstr[1] = ""; resulstr[2] = ""; resulstr[3] = "";
            }

            Char[,] thehasharray2= new char[64, 32];

            string hashpath = @"/Users/Ayas/Desktop/vis/hasharrays.txt";

            thehasharray2 = readHashArray(hashpath, 32);

            string alpha = arr[63, 0];
            string beta  = arr[63, 1];
            string teta  = arr[63, 2];
            string gamma = arr[63, 3];

            string st2 = "";
            for (int i = 0; i < 64; i++)
            {
                for (int j = 0; j < 32; j++)
                {
                    if (alpha.Substring(j, 1) == thehasharray2[i, j].ToString())
                    {
                        st2 += "1";
                    }
                    else { st2 += "0"; }
                }

                alpha = beta;
                beta = teta;
                teta = gamma;
                gamma = st2;
                st2 = "";
            }

            string res = "";

            for (int i = 0; i < 32; i += 8)
            {
                Int32 p1 = Convert.ToInt32(alpha.Substring(i, 8), 2);
                Int32 p2 = Convert.ToInt32(beta.Substring(i, 8), 2);
                Int32 p3 = Convert.ToInt32(teta.Substring(i, 8), 2);
                Int32 p4 = Convert.ToInt32(gamma.Substring(i, 8), 2);

                if (p1 < 32) { p1 += 32; }
                if (p2 < 32) { p2 += 32; }
                if (p3 < 32) { p3 += 32; }
                if (p4 < 32) { p4 += 32; }

                char c1 = Convert.ToChar(p1);
                char c2 = Convert.ToChar(p2);
                char c3 = Convert.ToChar(p3);
                char c4 = Convert.ToChar(p4);
                res = res + (c1).ToString() + (c2).ToString() + (c3).ToString() + (c4).ToString();
            }

            return res;
        }

        public static string runHashfunction(string input, Char[,] thehasharray) {

         
            int bytecount = ByteCount(input);
            int bitcount = BitCount(input);

            while(bitcount < 128) {
                input += "-";
                bitcount = BitCount(input);
            }

            while (bitcount % 128 != 0) {
                input += "-";
                bitcount = BitCount(input);
            }

            int p128count = bitcount / 128;

            byte[] bytes = Encoding.ASCII.GetBytes(input);

            byte[,,] p1 = new byte[p128count, 4, 4];
            //8888 8888 8888 8888
            // it's 16 bytes, 128 bits.
            int ac = 0;
            for (int i = 0; i < p128count; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    for (int k = 0; k < 4; k++)
                    {
                        p1[i, j, k] = bytes[ac];
                        ac++;
                        //  Console.Write(p1[i, j, k]);
                        //    if (k != 3) { Console.Write(" - "); }
                    }
                    //  Console.WriteLine();
                }
            }

            //Int32 mynum = 0b0111_1000_0010_0010_0011_1101_1111_1100;
            //Int32 mynum = 0b01111000001000100011110111111100;


            byte[,] alphas = new byte[p128count, 4];
            byte[,] betas  = new byte[p128count, 4];
            byte[,] tetas  = new byte[p128count, 4];
            byte[,] gammas = new byte[p128count, 4];


            for (int i = 0; i < p128count; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    alphas[i, j] = p1[i, 0, j];
                    betas[i, j] = p1[i, 1, j];
                    tetas[i, j] = p1[i, 2, j];
                    gammas[i, j] = p1[i, 3, j];
                }
               // Console.WriteLine();
            }


            byte[] theAlpha = new byte[4];
            byte[] theBeta = new byte[4];
            byte[] theTeta = new byte[4];
            byte[] theGamma = new byte[4];

            if (p128count > 1) {
                //xnor between same parts.

                byte[,] arr1 = new byte[4,4];
                byte[,] arr2 = new byte[4,4];

                byte[,] result = new byte[4,4];

                for (int i = 0; i < p128count-1; i++)
                {
                    for (int j = 0; j < 4; j++)
                    {
                        arr1[0,j] = alphas[i, j];
                        arr2[0,j] = alphas[(i+1), j];

                        arr1[1, j] = betas[i, j];
                        arr2[1, j] = betas[(i + 1), j];

                        arr1[2, j] = tetas[i, j];
                        arr2[2, j] = tetas[(i + 1), j];

                        arr1[3, j] = gammas[i, j];
                        arr2[3, j] = gammas[(i + 1), j];
                    }

                    for (int k = 0; k < 4; k++)
                    {
                        byte[] temp1 = new byte[4];
                        byte[] temp2 = new byte[4];

                        for (int z = 0; z < 4; z++)
                        {
                            temp1[z] = arr1[k, z];
                            temp2[z] = arr2[k, z];
                        }

                        char[] alp1 = ToBinary(temp1).ToCharArray();
                        char[] alp2 = ToBinary(temp2).ToCharArray();

                        string str3 = "";
                        for (int j = 0; j < 32; j++)
                        {
                            str3 = (alp1[j] == alp2[j]) ? (str3 + "1") : (str3 + "0");
                        }

                        for (int l = 0; l < 4; l++)
                        {
                            Int32 val = Convert.ToInt32(str3.Substring((l * 8), 8), 2);

                            if (k == 0)
                            {
                                alphas[(i + 1), l] = (byte)val;
                                result[k, l] = (byte)val;
                            }
                            else if (k == 1) {
                                betas[(i + 1), l] = (byte)val;
                                result[k, l] = (byte)val;
                            }
                            else if (k == 2)
                            {
                                tetas[(i + 1), l] = (byte)val;
                                result[k, l] = (byte)val;
                            }
                            else if (k == 3)
                            {
                                gammas[(i + 1), l] = (byte)val;
                                result[k, l] = (byte)val;
                            }

                        }
                    }


                }

                for (int i = 0; i < 4; i++)
                {
                    theAlpha[i] = result[0,i];
                    theBeta[i]  = result[1,i];
                    theTeta[i]  = result[2,i];
                    theGamma[i] = result[3,i];
                }

            }
            else if (p128count == 1) {

                for (int i = 0; i < 4; i++)
                {
                    theAlpha[i] = alphas[0, i];
                    theBeta[i]  = betas[0, i];
                    theTeta[i]  = tetas[0, i];
                    theGamma[i] = gammas[0, i];
                }
            }


            for (int i = 0; i < thehasharray.GetLength(0); i++)
            {
                char[] alpha = ToBinary(theAlpha).ToCharArray();


                char[] readingtablerow = new char[32];

                for (int j = 0; j < thehasharray.GetLength(1); j++)
                {
                    readingtablerow[j] = thehasharray[i, j];
                }
                

                string xnorstr = "";
                for (int j = 0; j < 32; j++)
                {
                    xnorstr = (alpha[j] == readingtablerow[j]) ? (xnorstr + "1") : (xnorstr + "0");
                }



                for (int l = 0; l < 4; l++)
                {
                    Int32 val = Convert.ToInt32(xnorstr.Substring((l * 8), 8), 2);
                    theAlpha[l] = (byte)val;       
                }

                byte[] gammatemp = theGamma;
                //a b t g
                theGamma = theAlpha;
                theAlpha = theBeta;
                theBeta = theTeta;
                theTeta = gammatemp;

            }

            string res = "";
            res = outputCreator(theAlpha, theBeta, theTeta, theGamma);

            return res;

            //char is 1 byte, so every char is equals 1 character

            //xnor lar yapıldı


            //xor farklılık kontrolü, xnor benzerlik kontrolü

            //şimdi alpha 64x32(4byte)genişliğindeki array in 1x32 si ile xnor lanacak
            //ardından hepsi bir sola kayacak, yani beta yeni alpha olup, işlem görmüş alpha yeni gamma olacak
            //bu operasyon 64 kere yapıldıktan sonra cryptografic hash elde edilmiş olacak


        }

        public static string outputCreator(byte[] theAlpha, byte[] theBeta, byte[] theTeta, byte[] theGamma) {


            string res = "";
            for (int i = 0; i < 4; i++)
            {
                char[] character1 = new char[8];
                char[] character2 = new char[8];
                char[] character3 = new char[8];
                char[] character4 = new char[8];

                char[] thearr = new char[32];
                if (i == 0)
                {
                    thearr = ToBinary(theAlpha).ToCharArray();
                }
                else if (i == 1)
                {
                    thearr = ToBinary(theBeta).ToCharArray();
                }
                else if (i == 2)
                {
                    thearr = ToBinary(theTeta).ToCharArray();
                }
                else if (i == 3)
                {
                    thearr = ToBinary(theGamma).ToCharArray();
                }

                for (int k = 0; k < 8; k++)
                {
                    character1[k] = thearr[k];
                    character2[k] = thearr[k + 8];
                    character3[k] = thearr[k + 16];
                    character4[k] = thearr[k + 24];
                }

                //Console.WriteLine(character1); //11001010
                string s1 = string.Join("", character1);
                string s2 = string.Join("", character2);
                string s3 = string.Join("", character3);
                string s4 = string.Join("", character4);
                Int32 a1 = Convert.ToInt32(s1, 2);
                Int32 a2 = Convert.ToInt32(s2, 2);
                Int32 a3 = Convert.ToInt32(s3, 2);
                Int32 a4 = Convert.ToInt32(s4, 2);

                res += Convert.ToChar(a1);
                res += Convert.ToChar(a2);
                res += Convert.ToChar(a3);
                res += Convert.ToChar(a4);

                // Console.WriteLine(System.Convert.ToInt32(s1));
                //Int32 a1 = Convert.ToInt32(s1, 2);

                // Console.WriteLine(a1);//202

                //Console.WriteLine(Convert.ToChar(a1)); //E
                // Console.WriteLine(Convert.ToChar(lowerportion));

                //theresultdata[acount] = hasheddata[i, j];

            }

            return res;
        }

        static void Main(string[] args)
        {
            Console.Clear();
            //xnor işlemlerini yap
            //okuduğun belgeden xnor lama yap
            //128 in katı değilse, ekleme işlemlerini yap belki bu otomatik yapılıyordur,
            /*
            //string Dataex = "2015510012";
            //string Dataex = "The Deu Ceng Hey hey";
            string Dataex = "The Deu Ceng Hey"; // it's 16 bytes, 128 bits.
            // string Dataex = "My student number is 2015510012*"; // it's 32 bytes, 256 bits.
            //string Dataex = "My name is Furkan Ayas and my student number is 2015510012 Dceng"; // it's 64 bytes, 512bits.
            string output = "";

            Char[,] thehasharray = new char[64, 32];

            string hashpath = @"/Users/Ayas/Desktop/vis/hasharrays.txt";

             thehasharray = readHashArray(hashpath, 32);
            */



            //string pdfFilePath = @"/Users/Ayas/Desktop/vis/dosya.pdf";
            //[Bernard_Kolman,_David_Hill]_Elementary_Linear_Alg(BookZZ.org)
            string pdfFilePath = @"/Users/Ayas/Desktop/vis/[Bernard_Kolman,_David_Hill]_Elementary_Linear_Alg(BookZZ.org).pdf";
            byte[] bytes = System.IO.File.ReadAllBytes(pdfFilePath);

            int _count = bytes.GetLength(0);
            //512byte to 16byte

            int _kbcount = _count / 1024;

            int artan = _count - (_kbcount * 1024);

            byte addition = 64;
            if (artan != 0)
            {
                while (artan % 1024 == 0)
                {
                    bytes.Append(addition);
                    artan = _count - (_kbcount * 1024);
                }
                _kbcount++;
            }
            byte[] sendingarr = new byte[1024]; //function made 1024byte 2byte.

            int count = 0;

            string send = "";
            var path = @"/Users/Ayas/Desktop/vis/output.txt";
            
            for (int j = 0; j < _kbcount; j++)
            {
                if (_count >= 1024)
                {

                    for (int i = 0; i < 1024; i++, count++)
                    {
                        sendingarr[i] = bytes[count];
                    }
                    _count -= 1024;
                }

                send = ToBinary(sendingarr);
                string ret = thesecondHashfunction(send);


                // This text is added only once to the file.
                if (!File.Exists(path))
                {
                    // Create a file to write to.
                    using (StreamWriter sw = File.CreateText(path))
                    {
                        sw.Write("");
                    }
                }

                using (StreamWriter sw = File.AppendText(path))
                {
                    sw.Write(ret);
                }
            }

            //Console.WriteLine(sa.Length);//8192byte 8kB. /16 byte = 512. 

            //Console.WriteLine(BitCount(sa));//65536

            Console.WriteLine("Output written external txt file!");

            /*
            //output = runHashfunction(Dataex, thehasharray);
            var path = @"/Users/Ayas/Desktop/vis/output.txt";


            // This text is added only once to the file.
            if (!File.Exists(path))
            {
                // Create a file to write to.
                using (StreamWriter sw = File.CreateText(path))
                {
                    sw.Write("");
                }
            }

            using (StreamWriter sw = File.AppendText(path))
            {
                sw.Write("");
                sw.Write(" ");
            }
                
            
            Console.WriteLine("Output written external txt file!");*/

            /*var path = @"/Users/Ayas/Desktop/vis/hey.txt";


            // This text is added only once to the file.
            if (!File.Exists(path))
            {
                // Create a file to write to.
                using (StreamWriter sw = File.CreateText(path))
                {
                    sw.Write("");
                    Console.WriteLine("");
                }
            }

            using (StreamWriter sw = File.AppendText(path))
            {
                sw.WriteLine("HEY");
                Console.WriteLine("");
            }*/

            /*
            string pdfFilePath = @"/Users/Ayas/Desktop/vis/dosya.pdf";
            byte[] bytes = System.IO.File.ReadAllBytes(pdfFilePath);
            Console.Clear();
            int byteNumber = bytes.GetLength(0);*/
            // my function result is 16 bytes 128 bit
            //input sizes 1kB 1024 bytes 8192 bit
            //64 times bigger.
            //readed pdf file 16 455 881 byte -> 16070 kB
            // operation 16070 kere tekrarlanacak

            //2^20 byte  2^4



            /*
            for (int j = 0; j < 5; j++)
            {
                for (int i = 0; i < 30; i++)
                {
                    Console.Write(bytes[i]);
                    Console.Write(" ");
                }
                Console.WriteLine("\n");
            }

            Console.WriteLine(bytes.GetLength(0));
            
            string x = (bytes.GetLength(0) / 1024).ToString();

            Console.WriteLine(x);
            */
            //Console.ReadLine();


            //Int32 mynum = 0b01111000001000100011110111111100;

            //Console.WriteLine(mynum);

            // Use any sort of encoding you like. 
            //    var binaryString = ToBinary(ConvertToByteArray("2015510012", Encoding.ASCII));



            /*
            long[] bina = new long[5];
            Int32 mynum = 0b0111_1000_0010_0010_0011_1101_1111_1100;
            //2,147,483,647
            Int32 lowest = 0b0000_0000_0000_0000_0000_0000_0000_0000;
            Int32 biggest = 0b0111_1111_1111_1111_1111_1111_1111_1111;
            */

            //createNewHashArray();

            //readHashArray();

            // a1 b1 c1 d1
            // a2 b2 c2 d2
            // a3 b3 c3 d3
            // a4 b4 c4 d4

            //xnor ((a1-a2)-a3)-a4) it make until blocks and
            //xnor ((b1-b2)-b3)-b4)
            //xnor ((c1-c2)-c3)-c4)
            //xnor ((d1-d2)-d3)-d4)

            //then create α (alpha) β (beta) γ(gamma) δ (delta)

            //then use first 32 of 64 on β
            //then slide all left. β γ δ α
            //then use second 32 ıf 64 on new β


            /*
             Step 1: Input size isn't important for function. Function divide data to 32 bits 4 parts.
             If the message is 256 bits, split it 2 pieces then starts on first 128 bits part then starts on second 128 bits part
             At the final add them in order.
             If the message is 100 bits, its add zeros at the end and make it 128bits
             If the message is 200 bits, function add zeros at the end make it 256 bits.
             So it make the number of multiple of 128.
             Get this multiplication number (for 1024 bit it will 8). Create bits array 32x8.

             It give xnor operation with this number all 32 bits columns.
             Then divide this 128 bits parts to 32 bits. Called Block "α (alpha) β (beta) γ(gamma) δ (delta)"

            EX;
            data ->  1011... 1110.. 0011. 01101.. (its equals 4 times 32 bits)
            get first array element of in 8 sized array
            that array contains 32 binary numbers.
            Gives xnor operations to β block and then slide all the blocks one right.
            At the next tour, new beta (gamma in the previous round) xnor with first arrays second binary number.
            After the 32 rounds. First 128 bits part is encrypted.

            Make this operation with second 128 bits part and use second array in 8 sized array. It also has 32 bits long binary

            Algorithm has 1024 sized array. and all 1024 arrays contain 32 bits binary numbers. 
             
             */
        }
    }
}
