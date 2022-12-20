using System.Text;

namespace Matrices.Matrices
{
    public class Matrix
    {
        private double[,] _matrix;

        public double[,] Matr
        {
            get => _matrix;
            protected init => _matrix = value;
        }

        public double this[int i, int j]
        {
            get
            {
                if (i > _matrix.GetLength(0) || j > _matrix.GetLength(1) || i < 0 || j < 0)
                    throw new Exception("Incorrect index");
                return _matrix[i, j];
            }
            set
            {
                if (i > _matrix.GetLength(0) || j > _matrix.GetLength(1) || i < 0 || j < 0)
                    throw new Exception("Incorrect index");
                _matrix[i, j] = value;
            }
        }

        protected Matrix()
        {
            _matrix = new double[1, 1];
        }

        protected Matrix(int m)
        {
            if (m <= 0) throw new Exception("Неверно заданы размерности");
            this._matrix = new double[m, m];
            for (var i = 0; i < m; i += 1)
                _matrix[i, i] = 1;
        }

        protected Matrix(int m, int n)
        {
            if (m <= 0 || n <= 0) throw new Exception("Неверно заданы размерности");
            _matrix = new double[m, n];
        }

        public Matrix(Matrix other)
        {
            _matrix = (double[,])other._matrix.Clone();
        }

        public Matrix(double[,] a)
        {
            var m = a.GetUpperBound(0) + 1;
            var n = a.Length / m;

            _matrix = new double[m, n];
            for (var i = 0; i < m; i += 1)
            {
                for (var j = 0; j < n; j += 1)
                {
                    _matrix[i, j] = a[i, j];
                }
            }
        }

        public int[] GetSize()
        {
            int[] size = new int[2];
            size[0] = _matrix.GetUpperBound(0) + 1;
            size[1] = _matrix.Length / size[0];
            return size;
        }

        public void ProcessFunctionOverMatrix(Action<int, int> func)
        {
            var size = GetSize();
            for (var i = 0; i < size[0]; i += 1)
            {
                for (var j = 0; j < size[1]; j += 1)
                {
                    func(i, j);
                }
            }
        }

        public Matrix CreateMatrixWithoutColumn(int column)
        {
            var size = GetSize();
            if (column < 0 || column >= size[1])
            {
                throw new ArgumentException("invalid column index");
            }
            var result = new Matrix(size[0], size[1] - 1);
            result.ProcessFunctionOverMatrix((i, j) => result[i, j] = j < column ? this[i, j] : this[i, j + 1]);
            return result;
        }

        protected Matrix CreateMatrixWithoutRow(int row)
        {
            var size = GetSize();
            if (row < 0 || row >= size[0])
            {
                throw new ArgumentException("invalid row index");
            }
            var result = new Matrix(size[0] - 1, size[1]);
            result.ProcessFunctionOverMatrix((i, j) => result[i, j] = i < row ? this[i, j] : this[i + 1, j]);
            return result;
        }

        protected static Matrix Swap(ref Matrix res, int row1, int row2)
        {
            var size = res.GetSize();
            for (var i = 0; i < size[1]; i += 1)
            {
                (res[row1, i], res[row2, i]) = (res[row2, i], res[row1, i]);
            }
            return res;
        }

        public static Matrix operator +(Matrix m1, Matrix m2)
        {
            var size1 = m1.GetSize();
            var size2 = m2.GetSize();
            if (!size1.SequenceEqual(size2)) throw new Exception("Матрицы имеют разные размерности");
            var res = new Matrix(size1[0], size1[1]);
            for (var i = 0; i < size1[0]; i += 1)
            {
                for (var j = 0; j < size1[1]; j += 1)
                {
                    res[i, j] = m1[i, j] + m2[i, j];
                }
            }
            return res;
        }

        public static Matrix operator +(Matrix m, double k)
        {
            var size = m.GetSize();
            var res = new Matrix(size[0], size[1]);

            for (var i = 0; i < size[0]; i += 1)
            {
                for (var j = 0; j < size[1]; j += 1)
                {
                    res[i, j] = m[i, j] + k;
                }
            }

            return res;
        }

        public static Matrix operator *(Matrix m, double v)
        {
            var size = m.GetSize();
            var res = new Matrix(size[0], size[1]);

            for (var i = 0; i < size[0]; i += 1)
            {
                for (var j = 0; j < size[1]; j += 1)
                {
                    res[i, j] = m[i, j] * v;
                }
            }

            return res;
        }

        public static Matrix operator -(Matrix m1, Matrix m2)
        {
            var size1 = m1.GetSize();
            var size2 = m2.GetSize();
            if (!size1.SequenceEqual(size2)) throw new Exception("Матрицы имеют разные размерности");

            var res = new Matrix(size1[0], size1[1]);
            res = m1 + (m2 * (-1));
            return res;
        }

        public static Matrix operator -(Matrix m, double v)
        {
            var res = new Matrix(m);

            return res + (-v);
        }

        public static Matrix operator *(Matrix m1, Matrix m2)
        {
            var size1 = m1.GetSize();
            var size2 = m2.GetSize();
            if (size1[1] != size2[0]) throw new Exception("Матрицы имеют неккоректные размерности");

            var res = new Matrix(size1[0], size2[1]);

            for (var i = 0; i < size1[0]; i += 1)
            {
                for (var j = 0; j < size2[1]; j += 1)
                {
                    for (var k = 0; k < size1[1]; k += 1)
                    {
                        res[i, j] += m1[i, k] * m2[k, j];
                    }
                }
            }

            return res;
        }

        public static Matrix operator /(Matrix m, double v)
        {
            var size = m.GetSize();
            var res = new Matrix(m);

            for (var i = 0; i < size[0]; i += 1)
            {
                for (var j = 0; j < size[1]; j += 1)
                {
                    res[i, j] /= v;
                }
            }

            return res;
        }

        public static Matrix operator ~(Matrix m) //Транспонирование
        {
            var size = m.GetSize();
            if (size[0] != size[1]) throw new Exception("Невозможно транспонировать матрицу");
            var res = new Matrix(size[0], size[1]);
            res.ProcessFunctionOverMatrix((i, j) => res[i, j] = m[j, i]);
            return res;
        }

        public override int GetHashCode()
        {
            return Matr.GetHashCode();
        }

        public static bool operator ==(Matrix m1, Matrix m2)
        {
            var size1 = m1.GetSize();
            var size2 = m2.GetSize();

            if (!size1.SequenceEqual(size2)) return false;

            for (var i = 0; i < size1[0]; i += 1)
            {
                for (var j = 0; j < size1[1]; j += 1)
                {
                    if (Math.Abs(m1[i, j] - m2[i, j]) > 1.00E-9) return false;
                }
            }

            return true;
        }

        public static bool operator !=(Matrix m1, Matrix m2)
        {
            return !(m1 == m2);
        }

        public override string ToString()
        {
            var res = new StringBuilder();
            var size = GetSize();

            for (var i = 0; i < size[0]; i += 1)
            {
                for (var j = 0; j < size[1]; j += 1)
                {
                    if (Math.Abs(_matrix[i, j]) < 1.00E-9)
                    {
                        res.Append("0 ");
                        continue;
                    }
                    res.Append(_matrix[i, j]);
                    res.Append(' ');
                }
                res.Append('\n');
            }
            return res.ToString();
        }

        public override bool Equals(object obj)
        {
            if (obj is not Matrix other) return false;
            
            var size1 = GetSize();
            var size2 = other.GetSize();
            if(!size1.SequenceEqual(size2)) return false;
                   
            for(var i = 0; i < size1[0]; i += 1)
            {
                for(var j = 0; j < size1[1]; j += 1)
                {
                    if (Math.Abs(this[i, j] - other[i, j]) > 0.001) return false;
                }
            }
            return true;
        }
    }
}
    