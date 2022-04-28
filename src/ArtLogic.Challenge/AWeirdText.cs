﻿using System.Collections;
using System.Text;

namespace ArtLogic.Challenge;

public static class AWeirdText
{
    public static IEnumerable<int> WeirdEncode(string input)
    {
        for (var i = 0; i < input.Length; i+=4)
        {
            var stringBlock = i + 4 <= input.Length ? input.Substring(i, 4)
                : input[i..].PadRight(4, '\0'); 
            
            yield return EncodeByte(stringBlock);
        }
    }

    public static string WeirdDecode(IEnumerable<int> input)
    {
        return input.Aggregate(string.Empty, (current, i) => current + Decode(i));
    }

    private static string Decode(int input)
    {
        var bits = new BitArray(new[]{input});
        var decoded = new BitArray(32);

        var index = 0;
        for (var i = 0; i < 4; i++)
        {
            for (var j = 0; j < 8; j++)
            {
                var position = i + j * 4;
                decoded[index] = bits[position];
                index++;
            }    
        }
            
        var bytes = new byte[4];
        decoded.CopyTo(bytes, 0);

        return Encoding.ASCII.GetString(bytes).Replace("\0", string.Empty);
    }

    private static int EncodeByte(string input)
    {
        var encoded = new BitArray(32);

        for (var i = 0; i < 4; i++)
        {
            var bytes = BitConverter.GetBytes(input[i]);

            var bits = new BitArray(bytes);
            for (var j = 0; j < 8; j++)
            {
                var position = i + j * 4;
                encoded[position] = bits[j];
            }
        }

        var result = new int[1];
        encoded.CopyTo(result, 0);
        return result[0];
    }
}