using Matrices.Matrices;

namespace Matrices
{
    public static class Program
    {
        static void Main(string[] args)
        {
            double[,] matr1 = {
                { 1.0, 2.0, 3.0},
                { 7.0, 3.0, 8.0},
                { 6.0, 5.0, 2.0} };
            double[,] matr2 =
            {
                {3.0, 1.0, 0.0 },
                {5.0, 6.0, 3.0 },
                {4.0, 2.0, 1.0 }
            };
            double[,] matr3 =
            {
                {1.0, 1.0, 1.0 },
                {4.0, 3.0, 2.0 }
            };
            double[,] matr4 =
            {
                {5, 4, -1},
                {10, 12, -3 },
                {0, 1, 1 }
            };
            double[,] matr5 =
            {
                {5, 4, -1, 2},
                {10, 12, -3, 4 },
                {0, 1, 1, 7 }
            };
            double[,] matr6 =
            {
                {1, 2 },
                {3, 4 }
            };



            var m1 = new Matrix(matr1);
            Console.WriteLine(m1);
            var m2 = new Matrix(matr2);
            Console.WriteLine(m2);
            var m3 = new Matrix(matr3);
            Console.WriteLine(m3);
            var m10 = new Matrix(matr4);
            Console.WriteLine(~m10);

            Console.WriteLine(m1 + m2);
            try
            {
                Console.WriteLine(m1 + m3);
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            try
            {
                Console.WriteLine(m1 * m3);
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            Console.WriteLine(m1 * m2);

            var sq1 = new SquaredMatrix(matr6);
            var i1 = new InvertedMatrix(sq1);

            Console.WriteLine(i1.Invert);
            Console.WriteLine(i1 * i1.Invert);
        }
    }
}