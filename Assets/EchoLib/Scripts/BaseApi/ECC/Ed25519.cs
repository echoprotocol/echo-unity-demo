using System;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Security.Cryptography;
using Tools.Hash;

namespace Cryptographic
{
    /* Ported and refactored from Java to C# by Hans Wolff, 10/10/2013
     * Released to the public domain
     * /
    /* Java code written by k3d3
     * Source: https://github.com/k3d3/ed25519-java/blob/master/ed25519.java
     * Released to the public domain
     */

    public class Ed25519
    {
        private const int BitLength = 256;

        private static readonly BigInteger TwoPowBitLengthMinusTwo = BigInteger.Pow(2, BitLength - 2);
        private static readonly BigInteger[] TwoPowCache = Enumerable.Range(0, 2 * BitLength).Select(i => BigInteger.Pow(2, i)).ToArray();

        private static readonly BigInteger Q =
            BigInteger.Parse("57896044618658097711785492504343953926634992332820282019728792003956564819949");

        private static readonly BigInteger Qm2 =
            BigInteger.Parse("57896044618658097711785492504343953926634992332820282019728792003956564819947");

        private static readonly BigInteger Qp3 =
            BigInteger.Parse("57896044618658097711785492504343953926634992332820282019728792003956564819952");

        private static readonly BigInteger L =
            BigInteger.Parse("7237005577332262213973186563042994240857116359379907606001950938285454250989");

        private static readonly BigInteger D =
            BigInteger.Parse("-4513249062541557337682894930092624173785641285191125241628941591882900924598840740");

        private static readonly BigInteger I =
            BigInteger.Parse("19681161376707505956807079304988542015446066515923890162744021073123829784752");

        private static readonly BigInteger By =
            BigInteger.Parse("46316835694926478169428394003475163141307993866256225615783033603165251855960");

        private static readonly BigInteger Bx =
            BigInteger.Parse("15112221349535400772501151409588531511454012693041857206046113283949847762202");

        private static readonly Tuple<BigInteger, BigInteger> B = new Tuple<BigInteger, BigInteger>(Bx.Mod(Q), By.Mod(Q));

        private static readonly BigInteger Un =
            BigInteger.Parse("57896044618658097711785492504343953926634992332820282019728792003956564819967");

        private static readonly BigInteger Two = new BigInteger(2);
        private static readonly BigInteger Eight = new BigInteger(8);


        private static byte[] ComputeHash(byte[] m)
        {
            using (var sha512 = SHA512Managed.Create())
            {
                return sha512.ComputeHash(m);
            }
        }

        private static BigInteger ExpMod(BigInteger number, BigInteger exponent, BigInteger modulo)
        {
            if (exponent.Equals(BigInteger.Zero))
            {
                return BigInteger.One;
            }
            var t = BigInteger.Pow(ExpMod(number, exponent / Two, modulo), 2).Mod(modulo);
            if (!exponent.IsEven)
            {
                t *= number;
                t = t.Mod(modulo);
            }
            return t;
        }

        private static BigInteger Inv(BigInteger x)
        {
            return ExpMod(x, Qm2, Q);
        }

        static Tuple<BigInteger, BigInteger> Edwards(BigInteger px, BigInteger py, BigInteger qx, BigInteger qy)
        {
            var xx12 = px * qx;
            var yy12 = py * qy;
            var dtemp = D * xx12 * yy12;
            var x3 = (px * qy + qx * py) * (Inv(1 + dtemp));
            var y3 = (py * qy + xx12) * (Inv(1 - dtemp));
            return new Tuple<BigInteger, BigInteger>(x3.Mod(Q), y3.Mod(Q));
        }

        static Tuple<BigInteger, BigInteger> EdwardsSquare(BigInteger x, BigInteger y)
        {
            var xx = x * x;
            var yy = y * y;
            var dtemp = D * xx * yy;
            var x3 = (2 * x * y) * (Inv(1 + dtemp));
            var y3 = (yy + xx) * (Inv(1 - dtemp));
            return new Tuple<BigInteger, BigInteger>(x3.Mod(Q), y3.Mod(Q));
        }

        static Tuple<BigInteger, BigInteger> ScalarMul(Tuple<BigInteger, BigInteger> p, BigInteger e)
        {
            if (e.Equals(BigInteger.Zero))
            {
                return new Tuple<BigInteger, BigInteger>(BigInteger.Zero, BigInteger.One);
            }
            var q = ScalarMul(p, e / Two);
            q = EdwardsSquare(q.Item1, q.Item2);
            if (!e.IsEven)
            {
                q = Edwards(q.Item1, q.Item2, p.Item1, p.Item2);
            }
            return q;
        }

        static byte[] EncodeInt(BigInteger y)
        {
            var nin = y.ToByteArray();
            var nout = new byte[Math.Max(nin.Length, 32)];
            Array.Copy(nin, nout, nin.Length);
            return nout;
        }

        static byte[] EncodePoint(BigInteger x, BigInteger y)
        {
            var nout = EncodeInt(y);
            nout[nout.Length - 1] |= (x.IsEven ? (byte)0 : (byte)0x80);
            return nout;
        }

        private static int GetBit(byte[] h, int i)
        {
            return h[i / 8] >> (i % 8) & 1;
        }

        public static byte[] PublicKey(byte[] signingKey)
        {
            var hash = SHA512.Create().HashAndDispose(signingKey);
            var a = TwoPowBitLengthMinusTwo;
            for (var i = 3; i < (BitLength - 2); i++)
            {
                var bit = GetBit(hash, i);
                if (bit != 0)
                {
                    a += TwoPowCache[i];
                }
            }
            var bigA = ScalarMul(B, a);
            return EncodePoint(bigA.Item1, bigA.Item2);
        }
        //# Converts a privkey into a pubkey
        //def privtopub(k):
        //    h = sha3_512(k)
        //    a = 2 ** (BITS - 2) + (decode_int(h[:32]) % 2 ** (BITS - 2))
        //    a -= (a % 8)
        //    return fast_multiply(B, a)
    }


    internal static class BigIntegerHelpers
    {
        public static BigInteger Mod(this BigInteger num, BigInteger modulo)
        {
            var result = num % modulo;
            return result < 0 ? result + modulo : result;
        }
    }
}