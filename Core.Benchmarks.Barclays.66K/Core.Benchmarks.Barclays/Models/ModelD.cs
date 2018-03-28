using System;

namespace Core.Benchmarks.Barclays.Models
{
    public class ModelD
    {
        public string Field1;
        public string Field2;
        public string Field3;
        public string Field4;
        public string Field5;

        public int Field6;
        public int Field7;
        public int Field8;
        public int Field9;
        public int Field10;

        public double Field11;
        public double Field12;
        public double Field13;
        public double Field14;
        public double Field15;

        public long Field16;
        public long Field17;
        public long Field18;
        public long Field19;
        public long Field20;

        public ModelD()
        {
        }

        public ModelD(Random random)
        {
            Field1 = random.Next().ToString().PadLeft(20);
            Field2 = random.Next().ToString().PadLeft(20);
            Field3 = random.Next().ToString().PadLeft(20);
            Field4 = random.Next().ToString().PadLeft(20);
            Field5 = random.Next().ToString().PadLeft(20);

            Field6 = random.Next();
            Field7 = random.Next();
            Field8 = random.Next();
            Field9 = random.Next();
            Field10 = random.Next();

            Field11 = random.NextDouble();
            Field12 = random.NextDouble();
            Field13 = random.NextDouble();
            Field14 = random.NextDouble();
            Field15 = random.NextDouble();

            Field16 = random.Next();
            Field17 = random.Next();
            Field18 = random.Next();
            Field19 = random.Next();
            Field20 = random.Next();
        }
    }
}
