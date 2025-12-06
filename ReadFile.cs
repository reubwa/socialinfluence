namespace InfluenceScore
{
    internal class ReadFile
    {
        public static string[,] ReadCsv()
        {
            //****************Important******************
            //In the solution explorer, right click on iso-country-codes.csv
            //Open properties 
            //Set Copy to Output Directory to Copy always
            //Otherwise, file wouldnot be read.

            Console.Write("Enter file name (including extension): ");

            string fn = Console.ReadLine();

            string filename = fn;

            StreamReader sr = null;
            string[,] data = null;

            if (File.Exists(filename))
            {
                sr = new StreamReader(File.OpenRead(filename));
                string line = null;
                int row = 0, col = 0;

                //since we use array data structure, we need to know the size for row and col.
                while (!sr.EndOfStream)
                {
                    row++;
                    line = sr.ReadLine();
                    var val = line.Split(',');
                    col = val.Length;
                }

                data = new string[row, col];  // to exclude the header

                sr = new StreamReader(File.OpenRead(filename));
                int i = 0;
                while (!sr.EndOfStream)
                {
                    var ln = sr.ReadLine();
                    var val = ln.Split(',');

                    for (int j = 0; j < val.Length; j++)
                    {
                        if (j < col)
                        {
                            data[i, j] = val[j];
                        }
                    }
                    i++;
                }
            }
            return data;
        }
    }
}
