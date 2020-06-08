using System;
using System.Runtime.ExceptionServices;

namespace NewNeuralNetwork
{
    class neuron
    {
        static void Main(string[] args)
        {

            neuralnetwork asd = new neuralnetwork(3, 3, 2, 2);
            double[] output = new double[2];


            // the input values
            double[] x1 = { 0, 0 };
            double[] x2 = { 0, 1 };
            double[] x3 = { 1, 0 };
            double[] x4 = { 1, 1 };




            // desired results
            double[] results = { 0, 1, 1, 0 };

            int epoch = 0;

        Retry:
            epoch++;
            for (int i = 0; i < 4; i++)  // very important, do NOT train for only one example
            {
                // 1) forward propagation (calculates output)
                double[] qwe = new double[2];
                if (i == 0)
                {
                    output = asd.run(x1);
                    Console.WriteLine("{0} xor {1} = {2} ------- {3}", x1[0], x1[1], output[0], output[1]);
                    qwe[0] = 0;
                    qwe[1] = 1;
                    asd.bp(qwe);

                }
                if (i == 1)
                {
                    output = asd.run(x2);
                    Console.WriteLine("{0} xor {1} = {2} ------- {3}", x2[0], x2[1], output[0], output[1]);
                    qwe[0] = 1;
                    qwe[1] = 0;
                    asd.bp(qwe);

                }
                if (i == 2)
                {
                    output = asd.run(x3);
                    Console.WriteLine("{0} xor {1} = {2} ------- {3}", x3[0], x3[1], output[0], output[1]);
                    qwe[0] = 1;
                    qwe[1] = 0;
                    asd.bp(qwe);

                }
                if (i == 3)
                {
                    output = asd.run(x4);
                    Console.WriteLine("{0} xor {1} = {2} ------- {3}", x4[0], x4[1], output[0], output[1]);
                    qwe[0] = 0;
                    qwe[1] = 1;
                    asd.bp(qwe);

                }

            }

            if (epoch < 500000)
                goto Retry;

            Console.WriteLine("asdadasdasdasdasdasdasdasdasd");

            for (int i = 0; i < 4; i++)  // very important, do NOT train for only one example
            {
                // 1) forward propagation (calculates output)
                double[] qwe = new double[1];
                if (i == 0)
                {
                    output = asd.run(x1);
                    Console.WriteLine("{0} xor {1} = {2} ------- {3}", x1[0], x1[1], output[0], output[1]);
                }
                if (i == 1)
                {
                    output = asd.run(x2);
                    Console.WriteLine("{0} xor {1} = {2} ------- {3}", x2[0], x2[1], output[0], output[1]);
                }
                if (i == 2)
                {
                    output = asd.run(x3);
                    Console.WriteLine("{0} xor {1} = {2} ------- {3}", x3[0], x3[1], output[0], output[1]);

                }
                if (i == 3)
                {
                    output = asd.run(x4);
                    Console.WriteLine("{0} xor {1} = {2} ------- {3}", x4[0], x4[1], output[0], output[1]);
                }

            }
            Console.ReadLine();
        }

        static Random rnd = new Random();

        public double[] Weights;
        public double bias;
        public double value = 0;
        public double error = 0;
        public neuron[] neuronsOut;

        public double[] errorArr;
        public double[] influence;
        public double[,] influence2;

        int FinaleoutputLength;

        int outputLength;

        public neuron(int outputLength, int FinaleoutputLength)
        {
            this.outputLength = outputLength;
            neuronsOut = new neuron[outputLength];
            errorArr = new double[FinaleoutputLength];
            influence = new double[FinaleoutputLength];
            influence2 = new double[outputLength, FinaleoutputLength];

            this.FinaleoutputLength = FinaleoutputLength;
            build();
        }
        void build()
        {
            Weights = new double[outputLength];
            for (int i = 0; i < outputLength; i++)
            {
                Weights[i] = 2 * rnd.NextDouble() - 1;
            }
            bias = 2 * rnd.NextDouble() - 1;
        }

        public void neuronsOutFill(neuron x, int place)
        {
            neuronsOut[place] = x;
        }
        public void Activation_function() { value = 1.0 / (1.0 + Math.Exp(-value)); }
        public double derivative() { return value * (1 - value); }

        public void adjustWeights()
        {
            double sum = 0;
            double w = 0.1;
            for (int z = 0; z < outputLength; z++)
            {
                sum = 0;
                for (int j = 0; j < FinaleoutputLength; j++)
                {
                    sum += errorArr[j] * influence2[z, j] * value;
                    //Console.WriteLine("                   "+ value + "    "+ influence2[z, j] + "    " + errorArr[j]);
                }
                //Console.WriteLine("                  sum         " + sum);
                //Console.WriteLine("                  B Weights         " + Weights[z]);
                Weights[z] += w * (sum / FinaleoutputLength);
                //Console.WriteLine("                  A Weights         " + Weights[z]);

            }
            sum = 0;
            for (int j = 0; j < FinaleoutputLength; j++)
            {
                sum += errorArr[j] * influence[j] * derivative();
            }
            bias += w * (sum / FinaleoutputLength);
        }
        public void initialization()
        {
            errorArr = new double[FinaleoutputLength];
            influence = new double[FinaleoutputLength];
            influence2 = new double[outputLength, FinaleoutputLength];
        }
    }

    //*****************************************************************************************************************************************************
    class neuralnetwork
    {
        public double score = 0;
        static Random rnd = new Random();
        public neuron[] entryLevel;
        public neuron[] outputLevel;
        public int length;
        public int numOfneuronsRows;
        public neuron[,] neurons;
        public double[,] bias;
        public int outputLength;
        public neuralnetwork(int numOfneuronsRows1, int length1, int entryLevelLenght, int outputLength)
        {
            numOfneuronsRows = numOfneuronsRows1;
            length = length1;
            this.outputLength = outputLength;
            entryLevel = new neuron[entryLevelLenght];
            outputLevel = new neuron[outputLength];
            neurons = new neuron[numOfneuronsRows, length];
            build();
        }
        void build()
        {
            for (int i = 0; i < entryLevel.Length; i++)
            {
                entryLevel[i] = new neuron(length, outputLength);
            }
            for (int i = 0; i < numOfneuronsRows; i++)
            {
                for (int j = 0; j < length; j++)
                {
                    if (i != numOfneuronsRows - 1)
                    {
                        neurons[i, j] = new neuron(length, outputLength);
                    }
                    else
                    {
                        neurons[i, j] = new neuron(outputLength, outputLength);
                    }
                }
            }
            
            for (int i = 0; i < entryLevel.Length; i++)
            {
                for (int j = 0; j < length; j++)
                {
                    entryLevel[i].neuronsOutFill(neurons[0, j], j);
                }
            }
            for (int i = 0; i < numOfneuronsRows; i++)
            {
                for (int j = 0; j < length; j++)
                {
                    if (i != numOfneuronsRows - 1)
                    {
                        for (int z = 0; z < length; z++)
                        {
                            neurons[i, j].neuronsOutFill(neurons[i + 1, z], z);
                        }
                    }
                }
            }

            for (int i = 0; i < outputLevel.Length; i++)
            {
                outputLevel[i] = new neuron(length, outputLength);
                for (int j = 0; j < length; j++)
                {
                    neurons[numOfneuronsRows - 1, j].neuronsOutFill(outputLevel[i], i);
                }
            }
        }
        void forward()
        {
            for (int i = 0; i < length; i++)
            {
                neurons[0, i].value = 0;
                for (int z = 0; z < entryLevel.Length; z++)
                {
                    neurons[0, i].value += entryLevel[z].value * entryLevel[z].Weights[i];
                }
                neurons[0, i].value += neurons[0, i].bias;
                neurons[0, i].Activation_function();
            }

            for (int i = 1; i < numOfneuronsRows; i++)
            {
                for (int j = 0; j < length; j++)
                {
                    neurons[i, j].value = 0;
                    for (int z = 0; z < length; z++)
                    {
                        neurons[i, j].value += neurons[i - 1, z].value * neurons[i - 1, z].Weights[j];
                    }
                    neurons[i, j].value += neurons[i, j].bias;
                    neurons[i, j].Activation_function();
                }
            }
            for (int i = 0; i < outputLevel.Length; i++)
            {
                outputLevel[i].value = 0;
                for (int z = 0; z < length; z++)
                {
                    outputLevel[i].value += neurons[numOfneuronsRows - 1, z].value * neurons[numOfneuronsRows - 1, z].Weights[i];
                }
                outputLevel[i].value += outputLevel[i].bias;

                outputLevel[i].Activation_function();
            }
        }
        public double[] run(double[] input)
        {
            for (int i = 0; i < entryLevel.Length; i++)
            {
                entryLevel[i].value = input[i];
            }
            forward();
            double[] output = new double[outputLevel.Length];

            for (int i = 0; i < outputLevel.Length; i++)
            {
                output[i] = outputLevel[i].value;
            }
            return (output);
        }
        //*********************************************************************************************************************************************************************************
        public void bp(double[] desiredResults)
        {

            for (int i = 0; i < outputLevel.Length; i++)
            {
                for (int j = 0; j < outputLevel.Length; j++)
                {
                    outputLevel[i].influence2[j, i] = 1;
                    outputLevel[i].errorArr[j] = (desiredResults[j] - outputLevel[j].value);
                }
                outputLevel[i].influence[i] = 1;
                //Console.WriteLine("output " + outputLevel[i].value);

            }

            for (int i = numOfneuronsRows - 1; i >= 0; i--)
            {
                for (int j = 0; j < length; j++)
                {
                    for (int z = 0; z < neurons[i, j].neuronsOut.Length; z++)
                    {
                        for (int m = 0; m < outputLevel.Length; m++)
                        {
                            if (z == 0)
                            {
                                //Console.WriteLine(i + " " + j + " " + outputLevel[z].errorArr[m] + "  " + neurons[i, j].neuronsOut[z].errorArr[m]);
                                neurons[i, j].errorArr[m] = neurons[i, j].neuronsOut[z].errorArr[m];
                            }
                            neurons[i, j].influence2[z, m] += neurons[i, j].neuronsOut[z].influence[m] * neurons[i, j].neuronsOut[z].derivative();
                        }
                    }
                    //Console.WriteLine(i + " " + j);
                    neurons[i, j].adjustWeights();
                    for (int z = 0; z < neurons[i, j].neuronsOut.Length; z++)
                    {
                        for (int m = 0; m < outputLevel.Length; m++)
                        {
                            neurons[i, j].influence[m] += neurons[i, j].neuronsOut[z].influence[m] * neurons[i, j].neuronsOut[z].derivative() * neurons[i, j].Weights[z];
                        }
                    }
                }
            }

            for (int i = 0; i < entryLevel.Length; i++)
            {
                for (int z = 0; z < length; z++)
                {
                    for (int m = 0; m < outputLevel.Length; m++)
                    {
                        if (z == 0)
                        {
                            entryLevel[i].errorArr[m] = entryLevel[i].neuronsOut[z].errorArr[m];
                        }
                        entryLevel[i].influence2[z, m] += entryLevel[i].neuronsOut[z].influence[m] * entryLevel[i].neuronsOut[z].derivative();
                    }
                }
                entryLevel[i].adjustWeights();
                for (int z = 0; z < length; z++)
                {
                    for (int m = 0; m < outputLevel.Length; m++)
                    {
                        entryLevel[i].influence[m] += entryLevel[i].neuronsOut[z].influence[m] * entryLevel[i].neuronsOut[z].derivative() * entryLevel[i].Weights[z];
                    }
                }
            }
            initialization();
        }

        private void initialization()
        {
            for (int i = 0; i < entryLevel.Length; i++)
            {
                entryLevel[i].initialization();
            }
            for (int i = 0; i < numOfneuronsRows; i++)
            {
                for (int j = 0; j < length; j++)
                {
                    neurons[i, j].initialization();
                }
            }

            for (int i = 0; i < outputLevel.Length; i++)
            {
                outputLevel[i].initialization();
            }
        }
    }
}
