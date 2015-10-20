using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication1
{
    class Program
    {
        static void Main(string[] args)
        {
            //try
            //{
            //    Trees.ListCompleteAutomorph(3);
            //}
            //catch (Exception ex)
            //{
            //    Console.WriteLine(ex.Message);
            //}
            //Console.WriteLine("2 " + Words.CountReducedWords(2));
            //Console.WriteLine("3 " + Words.CountReducedWords(3));
            //Console.WriteLine("4 " + Words.CountReducedWords(4));
            //Console.WriteLine("5 " + Words.CountReducedWords(5));
            //Console.WriteLine("6 " + Words.CountReducedWords(6))
            //Console.WriteLine("7 " + Words.CountReducedWords(7));
            //Console.WriteLine("8 " + Words.CountReducedWords(8));
            //Console.WriteLine("9 " + Words.CountReducedWords(9));
            //Console.WriteLine("10 " + Words.CountReducedWords(10));
            

            // List all permutations of the symmetric group S_n
            int n = 4;
            int k = 1;
            int j = 0;
            while (k <= General.Fact(n))
            {
                int[] perm = new int[n];
                perm = SymmetricGroup.Perm(n, k);
                j = 0;
                while (j < n - 1)
                {
                    Console.Write(perm[j] + " ");
                    j++;
                }
                Console.WriteLine(perm[j]);
                k++;
            }
            int count = 0;
            //count = Tanglegrams.FixedMaps(2, Trees.CompletePerm(4, 1), Trees.CompletePerm(4, 1));
            //Console.WriteLine(count);

            // Count the number of tanglegrams
            n = 2;
            int i = 1;
            j = 1;
            int treemorphs = Convert.ToInt32(Math.Pow(2,Math.Pow(2, n)-1));
            count = 0;
            while (i <= treemorphs)
            {
                while (j <= treemorphs)
                {
                    count = count + Tanglegrams.FixedMaps(n, Trees.CompletePerm(n, i), Trees.CompletePerm(n, j));
                    j++;
                }
                j = 1;
                i++;
            }
            int total = count / Convert.ToInt32(Math.Pow(treemorphs, 2));
            Console.WriteLine(total);
            Console.ReadKey();
        }
    }
    class Tanglegrams
    {
        /// <summary>
        /// Given an automorphism (f,g) of a tanglegram of complete trees with 2^n leaves, and given a permutation in S_(2^n) indentifying a tanglegram between them, this function computes the resulting tanglegram, relayed as a permutation in S_(2^n).
        /// </summary>
        /// <param name="map">A permutation of 2^n leaves representing a map between complete trees.</param>
        /// <param name="f">An automorphism of the left tree in one-line notation.</param>
        /// <param name="g">An automorphism of the right tree in one-line notation.</param>
        /// <returns></returns>
        public static int[] Automorph(int[] map, int[] f, int[] g)
        {
            // Initialize new permutaion
            int n = map.Length;
            int[] newmap = new int[n];

            // Compute action of tanglegram automorphism on map
            newmap = SymmetricGroup.Multiply(SymmetricGroup.Multiply(g, map), SymmetricGroup.Inverse(f));
            return newmap;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="map"></param>
        /// <returns></returns>
        public static int FixedMaps(int n, int[] f, int[] g)
        {
            int leaves = Convert.ToInt32(Math.Pow(2, n));
            int totalmaps = General.Fact(leaves);
            int i = 1;
            int count = 0;
            int[] map = new int[leaves];
            int[] newmap = new int[leaves];
            while (i <= totalmaps)
            {
                map = SymmetricGroup.Perm(leaves, i);
                newmap = Tanglegrams.Automorph(map, f, g);
                if (map.SequenceEqual(newmap))
                {
                    count++;
                }
                i++;
            }
            return count;
        }
    }
    class Trees
    {
        /// <summary>
        /// This function prints all permutations for a complete tree.  For a tree with 2^n leaves, input n.
        /// </summary>
        /// <param name="tree">The number of ancestors of each leaf.  (The "level" of the root where the leaves are level 0.)</param>
        /// <returns></returns>
        public static int ListCompleteAutomorph(int tree)
        {
            // Initialize counters
            int j = 0;
            int permnum = 1;  
            int[] perm = new int[Convert.ToInt32(Math.Pow(2, tree))];

            // Counts through all 2^(2^n-1) permutations
            while (permnum <= Math.Pow(2, Math.Pow(2, tree)-1))
            {
                perm = Trees.CompletePerm(tree, permnum);
                j = 0;
                // Prints permutation
                while (j < perm.Length - 1)
                {
                    Console.Write(perm[j] + " ");
                    j++;
                }
                Console.WriteLine(perm[j]);
                permnum++;
            }
            return 0;
        }
        /// <summary>
        /// This function will output an automorphism for a complete binary rooted tree in one-line notation as a permutation of the leaves.
        /// </summary>
        /// <param name="n">Designates a complete tree with 2^n leaves</param>
        /// <param name="k">Designates the k-th permutation, where k ranges from 1 to (2^n)-1.</param>
        /// <returns></returns>
        public static int[] CompletePerm(int n, int k)
        {
            ///              N7                          N7            
            ///            /    \                      /    \          
            ///           /      \                    /      \         
            ///         N6        N5                N5        N6       
            ///        / \       / \               / \       / \       
            ///       N4  N3    N2  N1            N1  N2    N3  N4     
            ///      / \  / \  / \  / \          / \  / \  / \  / \    
            ///      8 7  6 5  4 3  2 1          1 2  3 4  5 6  7 8    
            /// 
            /// The trees leaves are numbered 1 to 2^n from one end to the other.  The internal vertices and root are numbered first along the leaves' parents, then along the leaves' grandparents as shown in the example above.
            /// The input k is written in binary and the root and each internal vertex are associated to a digit.  The i-th node (Ni) is associated to the digit corresponding to 2^(i-1) in the binary expansion of k.  An automorphism is then generated by apply a symmetry at each node corresponding to a 1 in the binary expansion of k.
            /// 
            /// For example, given the tree above:
            /// k  binary  permutation
            /// ======================
            /// 0       0     [12345678]
            /// 1       1     [21345678]
            /// 2      10     [12435678]
            /// 3      11     [21435678]
            /// 4     100     [12346578]
            /// 16  10000     [34125678]
            /// 26  11010     [43125687]
            /// 
            //Check for valid input
            if (n < 1)
            {
                throw new ApplicationException("Invalid tree size, n.  Please choose a positive integer.");
            }
            else if (k < 1 | k > Math.Pow(2, Math.Pow(2, n) - 1))
            {
                throw new ApplicationException("Invalid permutation selection, k.  Please choose integer between 1 and " + Math.Pow(2, Math.Pow(2, n) - 1) + ".");
            }
            else
            {
                int leaves = Convert.ToInt32(Math.Pow(2, n));
                int[] permutationcode = Binary.Expansion(k - 1);

                //Initialize permuation to the identity
                int[] permutation = new int[leaves];
                int index = 0;
                while (index < leaves)
                {
                    permutation[index] = index + 1;
                    index++;
                }

                //Read binary code
                index = 0;
                while (index < permutationcode.Length)
                {
                    if (permutationcode[index] == 0)
                    {
                        index++;
                    }
                    else
                    {
                        //Identify the level of the internal vertex or root
                        int level = 1;
                        int vertices = index + 1;
                        bool arrived = false;
                        while (arrived == false)
                        {
                            if (vertices > Math.Pow(2, n - level))
                            {
                                vertices = vertices - Convert.ToInt32(Math.Pow(2, n - level));
                                level++;
                            }
                            else
                            {
                                arrived = true;
                            }
                        }

                        //Transpose the associated leaves
                        int subleaves = Convert.ToInt32(Math.Pow(2, level - 1));
                        int[] storage = new int[subleaves];

                        //Pull one set of leaves into storage
                        int i = 0;
                        while (i < subleaves)
                        {
                            storage[i] = permutation[i + 2 * subleaves * (vertices - 1)];
                            i++;
                        }

                        //Move second set of leaves to the first
                        i = 0;
                        while (i < subleaves)
                        {
                            permutation[i + 2 * subleaves * (vertices - 1)] = permutation[i + (2 * vertices - 1) * subleaves];
                            i++;
                        }

                        //Replace second set of leaves with the first from storage
                        i = 0;
                        while (i < subleaves)
                        {
                            permutation[i + (2 * vertices - 1) * subleaves] = storage[i];
                            i++;
                        }
                        index++;
                    }

                }
                return permutation;
            }
            
        }
    }
    class SymmetricGroup
    {
        /// <summary>
        /// This function converts a permutaion in one-line notation into its inverse in one-line notation.
        /// </summary>
        /// <param name="sigma">A permutation in one-line notation given as an integer array.</param>
        /// <returns></returns>
        public static int[] Inverse(int[] sigma)
        {
            // Calculate length
            int n = sigma.Length;

            // Initialize new permutation as the identity (used to verify function input)
            int[] phi = new int[n];
            int index = 0;
            while (index < n)
            {
                phi[index] = index + 1;
                index++;
            }

            // Verify the input defines a permutation
            int[] sigmacheck = new int[n];
            Array.Copy(sigma, sigmacheck, sigma.Length);
            Array.Sort(sigmacheck);
            if (!sigmacheck.SequenceEqual(phi))
            {
                throw new ApplicationException("Array does not define a permutation.  Please check for missing or repeat entries.");
            }

            // Convert new permutation to inverse
            else
            {
                index = 0;
                while (index < n)
                {
                    phi[sigma[index]-1] = index + 1;
                    index++;
                }
                return phi;
            }
        }
        /// <summary>
        /// This function multiplies two permutations in one-line notation.
        /// </summary>
        /// <param name="alpha">A permuation in one-line notation as an array.  The left side of the product.</param>
        /// <param name="beta">A permuation in one-line notation as an array.  The right side fo the product.</param>
        /// <returns></returns>
        public static int[] Multiply(int[] alpha, int[] beta)
        {
            int n = alpha.Length;
            int[] gamma = new int[n];

            // Check if both inputs are the same length
            if (alpha.Length != beta.Length)
            {
                throw new ApplicationException("Permutations do not have the same length.  Multiplication is not defined.");
            }
            else
            {
                // Initialize an identity permutation
                int index = 0;
                while (index < n)
                {
                    gamma[index] = index + 1;
                    index++;
                }

                // Verify both inputs define permutations
                int[] alphacheck = new int[n];
                Array.Copy(alpha, alphacheck, alpha.Length);
                int[] betacheck = new int[n];
                Array.Copy(beta, betacheck, beta.Length);
                Array.Sort(alphacheck);
                Array.Sort(betacheck);
                if (!gamma.SequenceEqual(alphacheck) | !gamma.SequenceEqual(betacheck))
                {
                    throw new ApplicationException("Arrays do not define permutations.  Please check for missing or repeat entries.");
                }

                // Compute product
                else
                {
                    index = 0;
                    while (index < n)
                    {
                        gamma[index] = alpha[beta[index]-1];
                        index++;
                    }
                    return gamma;
                }
            }
        }
        /// <summary>
        /// This funtion generates the k-th permutaion in the symmetric group S_n, where permutations are list in lexicographical order.
        /// </summary>
        /// <param name="n">The number of objects being permuted.</param>
        /// <param name="k">Identifies which permutation on the list should be generated.</param>
        /// <returns></returns>
        public static int[] Perm(int n, int k)
        {
            // Initialize a list of integers 1 through n to be placed in a permutation
            int[] list = new int[n];
            int index = 0;
            while (index < n)
            {
                list[index] = index + 1;
                index++;
            }

            // Initialize permutation array
            int[] perm = new int[n];

            // Place next integer
            index = 0;
            int relative;
            int j = 0;
            while (index < n)
            {
                // Identify the next integer, relatively
                relative = (k - 1) / General.Fact(n - 1 - index); // Integer division automatically rounds down
                perm[index] = list[relative];
                k = k - relative * General.Fact(n - 1 - index);

                // Shift the list elements forward
                j = relative;
                while (j < n-1)
                {
                    list[j] = list[j+1];
                    j++;
                }
                index++;
            }
            return perm;
        }
    }
    class General
    {
        /// <summary>
        /// This function calculates the factorial of a non-negative integer.
        /// </summary>
        /// <param name="n"></param>
        /// <returns></returns>
        public static int Fact(int n)
        {
            // Check for valid input.  Returns 1 if negative integer is given.
            if (n < 1)
            {
                return 1;
            }
            else
            {
                // Initialize product and loop to compute n!
                int fact = 1;
                while (n > 0)
                {
                    fact = fact * n;
                    n--;
                }
                return fact;
            }
        }
        public static bool EqualArray(int[] a, int[] b)
        {
            if (a.Length != b.Length)
            {
                return false;
            }
            else
            {
                int i = 0;
                while (i < a.Length)
                {
                    if (a[i] != b[i])
                    {
                        return false;
                    }
                }
                return true;
            }
        }
    }
    class Binary
    {
        /// <summary>
        /// Given an integer, this function outputs an array for the (nonnegative) binary expansion of that integer.
        /// </summary>
        /// <param name="Integer">Any integer.</param>
        /// <returns></returns>
        public static int[] Expansion(int Integer)
        {
            // Eliminate negative sign if any
            if(Integer < 0)
            {
                Integer = Integer * -1;
            }

            // Stop if zero
            if(Integer == 0)
            {
                return new int[0];
            }

            // Calculate the length needed for the binary array
            int arraylength = 1;
            bool maxsize = false;
            while (maxsize == false)
            {
                if (Integer / (Math.Pow(2, arraylength)) >= 1)
                {
                    arraylength++;
                }
                else
                {
                    maxsize = true;
                }
            }
            
            // Create the binary expansion array
            int[] binaryexpansion = new int[arraylength];

            // Fill the binary expansion array
            int i = arraylength - 1;
            while (i >= 0)
            {
                if (Integer / (Math.Pow(2,i)) >= 1)
                {
                    binaryexpansion[i] = 1;
                    Integer = Integer - Convert.ToInt32(Math.Pow(2, i));
                    i--;
                }
                else
                {
                    binaryexpansion[i] = 0;
                    i--;
                }
            }
            
            return binaryexpansion;
        }
    }
    class Words
    {
        /// <summary>
        /// This function counts the number of reduced words of the reverse permutatuion [n n-1 ... 3 2 1].
        /// Note this is very inefficient for n>= 6.  Increment function needs to skip over words with adjacent inverses.
        /// </summary>
        /// <param name="n"></param>
        /// <returns></returns>
        public static int CountReducedWords(int n)
        {
            int[] word = new int[n * (n - 1) / 2];
            int[] reverse = new int[n];
            int count = 0;

            //initialize the reverse permutation
            int index = 0;
            while (index < n)
            {
                reverse[index] = n - index;
                index++;
            }

            //initialize word
            index = 0;
            while (index < word.Length)
            {
                word[index] = 1;
                index++;
            }

            //check all words of same length
            index = 0;
            bool same = false;
            int j = 0;
            while (index<Math.Pow((n-1),word.Length))
            {
                int[] newperm = Words.Permutation(n, word);
                //compare permutations
                j = 0;
                same = true;
                while (j < n & same == true)
                {
                    if (reverse[j] == newperm[j])
                    {
                        j++;
                    }
                    else
                    {
                        same = false;
                    }
                }
                if (same == true)
                {
                    count = count +1;
                }
                word = Words.Increment(n, word);
                index++;
                same = false;
            }
            return count;

        }
        /// <summary>
        /// This function increments a word written in [n-1].
        /// </summary>
        /// <param name="n"></param>
        /// <param name="word"></param>
        /// <returns></returns>
        public static int[] Increment(int n, int[] word)
        {
            int index = word.Length - 1;
            bool end = false;
            while (end == false)
            {
                if (index == 0 & word[index] == n-1)
                {
                    end = true;
                }
                else if (word[index] < n - 1)
                {
                    word[index] = word[index] + 1;
                    end = true;
                }
                else
                {
                    word[index] = 1;
                    index--;
                }
            }
            return word;
        }
        /// <summary>
        /// This function outputs the one-line notation for a permuation given as a word.
        /// </summary>
        /// <param name="n"></param>
        /// <param name="word"></param>
        /// <returns></returns>
        public static int[] Permutation(int n, int[] word)
        {
            //initialize permutation
            int[] perm = new int[n];
            int j = 0;
            while (j < n)
            {
                perm[j] = j + 1;
                j++;
            }

            //calculate permutation from word
            int index = word.Length - 1;
            int temp = 0;
            while (index >= 0)
            {
                temp = perm[word[index]];
                perm[word[index]] = perm[word[index] - 1];
                perm[word[index] - 1] = temp;
                index--;
            }
            return perm;

        }
    }

}
