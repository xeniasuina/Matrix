namespace Matrices.Matrices
{
    internal class SquaredMatrix : Matrix
    {

        public double Det => Calculate();

        public SquaredMatrix() { }

        public SquaredMatrix(int m, int n) : base(m, n) { }
        public SquaredMatrix(double[,] matr)
        {
            if(matr.GetLength(0) != matr.GetLength(1))
            {
                throw new Exception("Матрица не квадратная. ");
            }
            Matr = new double[matr.GetLength(0), matr.GetLength(1)];
            Matr = (double[,])matr.Clone();
        }
        public SquaredMatrix(Matrix matr)
        {
            var size = matr.GetSize();
            if (size[0] != size[1]) throw new Exception("Матрица не квадратная.");
            Matr = new double[size[0], size[1]];
            Matr = (double[,])matr.Matr.Clone();
        }

        public SquaredMatrix(SquaredMatrix other)
        {
            var size = other.GetSize();
            Matr = new double[size[0], size[1]];
            Matr = (double[,])other.Matr.Clone();
        }

        public SquaredMatrix CreateMinor(int i, int j)
        {
            return new SquaredMatrix(CreateMatrixWithoutRow(i).CreateMatrixWithoutColumn(j));
        }

        public double Calculate()
        {
            var size = GetSize();
            if (size[0] != size[1]) throw new Exception("Матрица не квадратная. ");
            var n = size[0];
            if (n == 1) return this[0, 0];

            var res = new Matrix(this);
            var det = 1.0;
            const double EPS = 1.00E-9;
            
            for (var i = 0; i < n; i += 1)
            {
                var k = i;
                for (var j = i + 1; j < n; j += 1)
                {
                    if (Math.Abs(res[j, i]) > Math.Abs(res[k, i]))
                    {
                        k = j;
                    }
                }
                if (Math.Abs(res[k, i]) < EPS)
                {
                    det = 0;
                    break;
                }
                Swap(ref res, i, k);
                if (i != k)
                {

                    det *= -1;
                }
                det *= res[i, i];

                for (var j = i + 1; j < n; j += 1)
                {
                    res[i, j] /= res[i, i];
                }
                for (var j = 0; j < n; j += 1)
                {
                    if (j == i || !(Math.Abs(this[j, i]) > EPS)) continue;
                    
                    for (var s = i + 1; s < n; s += 1)
                    {
                        res[j, s] -= res[i, s] * res[j, i];
                    }
                }

            }
            return det;
        }

        public static SquaredMatrix operator /(SquaredMatrix sq1, double val)
        { 
            var res = new SquaredMatrix(sq1);
            res.ProcessFunctionOverMatrix((i, j) => res[i, j] /= val);
            return res;
        }

        public static SquaredMatrix operator~(SquaredMatrix sq1)
        {
            var res = new SquaredMatrix(sq1);
            res.ProcessFunctionOverMatrix((i, j) => res[i, j] = sq1[j, i]);
            return res;
        }

        public static SquaredMatrix operator *(SquaredMatrix sq1, SquaredMatrix sq2)
        {
            var size = sq1.GetSize();
            if (size[0] != size[1]) throw new Exception("Матрицы имеют разные размерности");
            var res = new SquaredMatrix(size[0], size[1]);
            
            res.ProcessFunctionOverMatrix((i, j) =>
            {
                for (var k = 0; k < size[0]; k += 1)
                {
                    res[i, j] += sq1[i, k] * sq2[k, j];
                }
            });
            return res;
        }
    }
}
