namespace Matrices.Matrices
{
    internal class InvertedMatrix : SquaredMatrix
    {
     
        public SquaredMatrix Invert
        {
            get { return GetInvertMatrix(); }
        }
        public InvertedMatrix() { }
        public InvertedMatrix(double[,] matr) : base(matr)
        {
            if (Det == 0) throw new Exception("Матрица вырожденная");
        }
        public InvertedMatrix(Matrix matr) : base(matr) {
            if (Det == 0) throw new Exception("Матрица вырожденная");
        }
        public InvertedMatrix(SquaredMatrix matr) : base(matr) {
            if (Det == 0) throw new Exception("Матрица вырожденная");
        }

        public InvertedMatrix(InvertedMatrix other)
        {
            var size = other.GetSize();
            Matr = new double[size[0], size[1]];
            Matr = (double[,])other.Matr.Clone();
        }

        private SquaredMatrix GetInvertMatrix()
        {
            var result = new SquaredMatrix(this);
            result.ProcessFunctionOverMatrix((i, j) =>
                result[i, j] = ((i + j) % 2 == 1 ? -1 : 1) * 
                               CreateMinor(i, j).Calculate());
            result /= Det;
            result = ~result;

            return result;
        }
    }
}