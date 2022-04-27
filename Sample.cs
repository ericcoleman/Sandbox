using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace ConsoleSandbox.Tests;

public class UnitTest1
{
    [Theory]
    [InlineData("A", new[] { 16777217})]
    [InlineData("FRED", new[] {251792692})]
    [InlineData("foo", new[] {124807030})]
    [InlineData(" foo", new[] {250662636})]
    [InlineData("foot", new[] {267939702})]
    [InlineData("BIRD", new[] {251930706})]
    [InlineData("....", new[] {15794160})]
    [InlineData("^^^^", new[] {252706800})]
    [InlineData("Woot", new[] {266956663})]
    [InlineData("no", new[] {53490482})]
    [InlineData("eric", new[] {267534765})]
    [InlineData("a@b.", new[] {131107009})]
    [InlineData("tacocat", new[] { 267487694, 125043731 })]
    [InlineData("never odd or even", new[] { 267657050, 233917524, 234374596, 250875466, 17830160 })]
    [InlineData("lager, sir, is regal", new[] { 267394382, 167322264, 66212897, 200937635, 267422503 })]
    [InlineData("go hang a salami, I'm a lasagna hog", new[] { 200319795, 133178981, 234094669, 267441422, 78666124, 99619077, 267653454, 133178165, 124794470 })]
    [InlineData("egad, a base tone denotes a bad age", new[] { 267389735, 82841860, 267651166, 250793668, 233835785, 267665210, 99680277, 133170194, 124782119 })]
    public void CanEncodeStrings(string input, int[] expectedOutput)
    {
        var encoded = AThing.CrazyEncode(input)
            .ToList();

        Assert.Equal(expectedOutput.Length, encoded.Count);
        
        for (var i = 0; i < expectedOutput.Length; i++)
        {
            Assert.Equal(expectedOutput[i], encoded[i]);
        }
    }
    
    [Theory]
    [InlineData("tacocat", new[] { 267487694, 125043731 })]
    [InlineData("never odd or even", new[] { 267657050, 233917524, 234374596, 250875466, 17830160 })]
    [InlineData("lager, sir, is regal", new[] { 267394382, 167322264, 66212897, 200937635, 267422503 })]
    [InlineData("go hang a salami, I'm a lasagna hog", new[] { 200319795, 133178981, 234094669, 267441422, 78666124, 99619077, 267653454, 133178165, 124794470 })]
    [InlineData("egad, a base tone denotes a bad age", new[] { 267389735, 82841860, 267651166, 250793668, 233835785, 267665210, 99680277, 133170194, 124782119 })]
    [InlineData("A", new[] { 16777217})]
    [InlineData("FRED", new[] {251792692})]
    [InlineData("foo", new[] {124807030})]
    [InlineData(" foo", new[] {250662636})]
    [InlineData("foot", new[] {267939702})]
    [InlineData("BIRD", new[] {251930706})]
    [InlineData("....", new[] {15794160})]
    [InlineData("^^^^", new[] {252706800})]
    [InlineData("Woot", new[] {266956663})]
    [InlineData("no", new[] {53490482})]
    [InlineData("eric", new[] {267534765})]
    [InlineData("a@b.", new[] {131107009})]
    public void CanDecodeStrings(string expected, int[] input)
    {
        var decoded = AThing.CrazyDecode(input);

        Assert.Equal(expected, decoded);
        
        Assert.Equal(expected, AThing.CrazyDecode(AThing.CrazyEncode(expected)));
    }

    private static class AThing
    {
        public static IEnumerable<int> CrazyEncode(string input)
        {
            for (var i = 0; i < input.Length; i+=4)
            {
                var stringBlock = i + 4 <= input.Length ? input.Substring(i, 4) : input[i..].PadRight(4, '\0'); 
                yield return EncodeByte(stringBlock);
            }
        }

        public static string CrazyDecode(IEnumerable<int> input)
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
}

