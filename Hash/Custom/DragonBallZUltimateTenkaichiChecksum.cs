using System;

/// <summary>
/// Copyright (c) 2016 Fredric Baeckström Arklid
/// All rights reserved.
/// 
/// THIS SOFTWARE IS PROVIDED "AS IS" AND ANY EXPRESSED OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS
/// FOR A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE REGENTS OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR 
/// CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION)
/// 
/// HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT(INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE 
/// USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
/// 
/// 0101010001101000011001010111001001100101001000000110100101110011001000000110111001101111001000000111001101110000011011110110111101101110
/// </summary>
namespace GameSecurityResearch.Hash.Custom
{
    public static class DragonBallZUltimateTenkaichiChecksum
    {
        private static uint LoadWord(byte[] Data)
        {
            return (Data[3]) | (uint)(Data[2] << 8) | (uint)(Data[1] << 16) | (uint)(Data[0] << 24);
        }

        private static void StoreWord(ref byte[] Data, uint Dword)
        {
            Data[3] = (byte)(Dword & 0xFF);
            Data[2] = (byte)((Dword & 0xFF00) >> 8);
            Data[1] = (byte)((Dword & 0xFF0000) >> 16);
            Data[0] = (byte)((Dword & 0xFF000000) >> 24);
        }

        private static byte LoadByte(byte[] Data, uint Offset)
        {
            return Data[Offset];
        }

        private static void DoRound(ref uint A, byte[] B, uint C, ref uint D, ref uint E, ref uint F)
        {
            if (A != 0)
            {
                D = LoadByte(B, C);
                E = (D << 8) | (D >> 24);
            }
            else
                E = LoadByte(B, C);

            F = E + F;

            if (F > -1)
                F++;
        }

        public static bool Calculate(byte[] Data, bool Fix)
        {
            uint dwXRound = 0, dwYRound = 0, dwFinalValue = 0;
            byte[] pbDataPointer4 = null, pbDataPointer3 = null, pbDataPointer1 = null, pbDataPointer2 = null;

            if (Fix)
            {
                StoreWord(ref Data, 0);
                StoreWord(ref Data + 4, 0);
            }

            pbDataPointer1 = Data + 8;
            pbDataPointer2 = pbDataPointer1 + 1;
            pbDataPointer3 = pbDataPointer1 + 2;
            pbDataPointer4 = pbDataPointer1 + 3;

            for (uint i = 0, dwIndexer = 0; i < 0x927A; i++, dwIndexer += 4)
            {
                uint dwTmpValue;

                dwXRound = dwIndexer & 1;
                pbDataPointer1 = pbDataPointer2 + dwIndexer;

                DoRound(ref dwXRound, pbDataPointer1, -1, ref dwYRound, dwTmpValue, ref dwFinalValue);

                dwTmpValue = dwIndexer - 1;
                dwYRound = dwTmpValue & 1;

                DoRound(ref dwYRound, pbDataPointer2, dwIndexer, ref dwTmpValue, ref dwTmpValue, ref dwFinalValue);
                DoRound(ref dwXRound, pbDataPointer3, dwIndexer, ref dwTmpValue, ref dwTmpValue, ref dwFinalValue);
                DoRound(ref dwYRound, pbDataPointer4, dwIndexer, ref dwTmpValue, ref dwTmpValue, ref dwFinalValue);
            }

            if (Fix)
            {
                uint CSum1 = dwFinalValue >> 8;
                uint CSum2 = dwFinalValue & 0xFF;

                StoreWord(Data, CSum1);
                StoreWord(Data + 4, CSum2);

                return true;
            }
            else
            {
                uint CSum1 = LoadWord(Data);
                uint CSum2 = LoadWord(Data + 4);

                uint Calc1 = dwFinalValue >> 8;
                uint Calc2 = dwFinalValue & 0xFF;

                if (CSum1 == Calc1 || CSum2 == Calc2)
                    return true;
            }

            return false;
        }
    }
}
