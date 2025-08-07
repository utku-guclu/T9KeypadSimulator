using System;
using Xunit;
using T9KeypadSimulator;

namespace T9KeypadSimulator.Tests
{
    /// <summary>
    /// Unit tests for the OldPhonePad functionality.
    /// Tests all the core features including basic letter mapping, separators, backspace, and edge cases.
    /// </summary>
    public class OldPhonePadTests
    {
        #region Requested Test Cases

        [Fact]
        public void OldPhonePad_RequestedTestCases()
        {
            // Test cases provided by the user
            Assert.Equal("CAB", OldPhonePadSimulator.OldPhonePad("222 2 22#"));
            Assert.Equal("E", OldPhonePadSimulator.OldPhonePad("33#"));
            Assert.Equal("B", OldPhonePadSimulator.OldPhonePad("227*#"));
            Assert.Equal("HELLO", OldPhonePadSimulator.OldPhonePad("4433555 555666#"));
            Assert.Equal("TURING", OldPhonePadSimulator.OldPhonePad("8 88777444666*664#"));
        }

        #endregion

        #region Basic Functionality Tests

        [Fact]
        public void OldPhonePad_SingleKeyPress_ReturnsFirstLetter()
        {
            // Arrange & Act & Assert
            Assert.Equal("A", OldPhonePadSimulator.OldPhonePad("2#"));
            Assert.Equal("D", OldPhonePadSimulator.OldPhonePad("3#"));
            Assert.Equal("G", OldPhonePadSimulator.OldPhonePad("4#"));
            Assert.Equal("J", OldPhonePadSimulator.OldPhonePad("5#"));
            Assert.Equal("M", OldPhonePadSimulator.OldPhonePad("6#"));
            Assert.Equal("P", OldPhonePadSimulator.OldPhonePad("7#"));
            Assert.Equal("T", OldPhonePadSimulator.OldPhonePad("8#"));
            Assert.Equal("W", OldPhonePadSimulator.OldPhonePad("9#"));
        }

        [Fact]
        public void OldPhonePad_MultipleKeyPresses_ReturnsCorrectLetter()
        {
            // Test second letters
            Assert.Equal("B", OldPhonePadSimulator.OldPhonePad("22#"));
            Assert.Equal("E", OldPhonePadSimulator.OldPhonePad("33#"));
            Assert.Equal("H", OldPhonePadSimulator.OldPhonePad("44#"));
            
            // Test third letters
            Assert.Equal("C", OldPhonePadSimulator.OldPhonePad("222#"));
            Assert.Equal("F", OldPhonePadSimulator.OldPhonePad("333#"));
            Assert.Equal("I", OldPhonePadSimulator.OldPhonePad("444#"));
        }

        [Fact]
        public void OldPhonePad_FourLetterKey_CyclesThroughAllLetters()
        {
            // Test fourth letter (key cycles back to first)
            Assert.Equal("S", OldPhonePadSimulator.OldPhonePad("7777#"));  // 4th press of 7 (P->Q->R->S->P)
            Assert.Equal("P", OldPhonePadSimulator.OldPhonePad("77777#")); // 5th press of 7 (back to first letter)
            
            // Test all four letters in sequence
            Assert.Equal("P", OldPhonePadSimulator.OldPhonePad("7#"));
            Assert.Equal("Q", OldPhonePadSimulator.OldPhonePad("77#"));
            Assert.Equal("R", OldPhonePadSimulator.OldPhonePad("777#"));
            Assert.Equal("S", OldPhonePadSimulator.OldPhonePad("7777#"));
            Assert.Equal("P", OldPhonePadSimulator.OldPhonePad("77777#")); // Cycles back to first letter
            
            // Test with key 9 which also has 4 letters
            Assert.Equal("W", OldPhonePadSimulator.OldPhonePad("9#"));
            Assert.Equal("X", OldPhonePadSimulator.OldPhonePad("99#"));
            Assert.Equal("Y", OldPhonePadSimulator.OldPhonePad("999#"));
            Assert.Equal("Z", OldPhonePadSimulator.OldPhonePad("9999#"));
            Assert.Equal("W", OldPhonePadSimulator.OldPhonePad("99999#")); // Cycles back to first letter
        }

        [Fact]
        public void OldPhonePad_ExcessKeyPresses_CyclesBackToBeginning()
        {
            // Test cycling behavior - more presses than available letters
            Assert.Equal("A", OldPhonePadSimulator.OldPhonePad("2222#")); // 4 presses, cycles back to A
            Assert.Equal("P", OldPhonePadSimulator.OldPhonePad("77777#")); // 5 presses on 4-letter key, cycles to P
        }

        #endregion

        #region Separator Tests

        [Fact]
        public void OldPhonePad_WordWithSpaces_WorksCorrectly()
        {
            // Test with spaces between words (0 is space)
            // Current implementation doesn't add spaces between characters from the same key press
            Assert.Equal("HI TERE", OldPhonePadSimulator.OldPhonePad("44 4440 8 33 777 33#"));
            
            // Test with multiple spaces
            Assert.Equal("A B", OldPhonePadSimulator.OldPhonePad("20 22#"));
            
            // Test with spaces and backspace - backspace removes the space
            // The input "4433555 5556660*#" translates to:
            // 44=H, 333=E, 555=LL, [space]=process sequence, 555666=LO, 0=space, *=backspace, #=end
            // Implementation processes the sequence before backspace, so it removes the space, not the O
            Assert.Equal("HELLO", OldPhonePadSimulator.OldPhonePad("4433555 5556660*#"));
            
            // Test with spaces at beginning and end
            // The implementation adds a trailing space because of the '0' at the end
            Assert.Equal(" HELLO ", OldPhonePadSimulator.OldPhonePad("04433555 5556660#"));
            
            // Test with multiple key presses on the same key
            Assert.Equal("CBA", OldPhonePadSimulator.OldPhonePad("222 22 2#"));
        }

        #endregion

        #region Backspace Tests

        [Fact]
        public void OldPhonePad_Backspace_RemovesLastCharacter()
        {
            // Test basic backspace - implementation processes the sequence before applying backspace
            Assert.Equal("", OldPhonePadSimulator.OldPhonePad("22*#")); // 22=B, *=backspace (removes B)
            Assert.Equal("", OldPhonePadSimulator.OldPhonePad("2*#"));   // 2=A, *=backspace (removes A)
        }

        [Fact]
        public void OldPhonePad_BackspaceAfterSequence_ProcessesSequenceThenRemoves()
        {
            // 22 = B, 7 = P, result is BP, then * removes P, leaving B
            Assert.Equal("B", OldPhonePadSimulator.OldPhonePad("227*#"));
        }

        [Fact]
        public void OldPhonePad_MultipleBackspaces_RemovesMultipleCharacters()
        {
            // Implementation processes the sequence before applying backspaces
            // 44=H, 333=E, **=backspace twice (removes E then H)
            Assert.Equal("", OldPhonePadSimulator.OldPhonePad("44333**#"));
            
            // 2=A, *=backspace (removes A)
            Assert.Equal("", OldPhonePadSimulator.OldPhonePad("2*#"));
        }

        [Fact]
        public void OldPhonePad_BackspaceOnEmptyResult_DoesNothing()
        {
            Assert.Equal("", OldPhonePadSimulator.OldPhonePad("*#"));
            Assert.Equal("", OldPhonePadSimulator.OldPhonePad("***#"));
        }

        #endregion

        #region Space Character Tests

        [Fact]
        public void OldPhonePad_ZeroKey_AddsSpaceCharacter()
        {
            Assert.Equal(" ", OldPhonePadSimulator.OldPhonePad("0#"));
            Assert.Equal("A B", OldPhonePadSimulator.OldPhonePad("2022#"));
        }

        [Fact]
        public void OldPhonePad_MultipleZeros_AddsMultipleSpaces()
        {
            Assert.Equal("  ", OldPhonePadSimulator.OldPhonePad("00#"));
            Assert.Equal("A  B", OldPhonePadSimulator.OldPhonePad("20022#"));
        }

        #endregion

        #region Complex Integration Tests

        [Fact]
        public void OldPhonePad_ProvidedExamples_WorkCorrectly()
        {
            // Test all examples from the requirements
            Assert.Equal("E", OldPhonePadSimulator.OldPhonePad("33#"));
            Assert.Equal("B", OldPhonePadSimulator.OldPhonePad("227*#"));
            Assert.Equal("HELLO", OldPhonePadSimulator.OldPhonePad("4433555 555666#"));
        }

        [Fact]
        public void OldPhonePad_ComplexWord_HELLO_BreakdownTest()
        {
            // Detailed breakdown of HELLO example
            // 44 = H, 33 = E, 555 = L, space separator, 555 = L, 666 = O
            var result = OldPhonePadSimulator.OldPhonePad("4433555 555666#");
            Assert.Equal("HELLO", result);
        }

        [Fact]
        public void OldPhonePad_MysteryExample_ShouldWorkWithCorrectLogic()
        {
            // This should spell "TURING" based on the pattern
            // 8 = T, space, 88 = U, 777 = R, 444 = I, 666 = N, *, 664 = G
            var result = OldPhonePadSimulator.OldPhonePad("8 88777444666*664#");
            Assert.Equal("TURING", result);
        }

        // Removed duplicate test method - keeping the more comprehensive version above

        #endregion

        #region Edge Cases

        [Fact]
        public void OldPhonePad_EmptyInput_ReturnsEmptyString()
        {
            Assert.Equal("", OldPhonePadSimulator.OldPhonePad(""));
            Assert.Equal("", OldPhonePadSimulator.OldPhonePad(null));
        }

        [Fact]
        public void OldPhonePad_OnlyHashSymbol_ReturnsEmptyString()
        {
            Assert.Equal("", OldPhonePadSimulator.OldPhonePad("#"));
        }

        [Fact]
        public void OldPhonePad_InputWithoutHash_ThrowsArgumentException()
        {
            Assert.Throws<ArgumentException>(() => OldPhonePadSimulator.OldPhonePad("22"));
            Assert.Throws<ArgumentException>(() => OldPhonePadSimulator.OldPhonePad("ABC"));
        }

        [Fact]
        public void OldPhonePad_InvalidCharacters_ThrowsArgumentException()
        {
            Assert.Throws<ArgumentException>(() => OldPhonePadSimulator.OldPhonePad("1#"));
            Assert.Throws<ArgumentException>(() => OldPhonePadSimulator.OldPhonePad("a#"));
            Assert.Throws<ArgumentException>(() => OldPhonePadSimulator.OldPhonePad("2a#"));
        }

        [Fact]
        public void OldPhonePad_NoHashSymbol_ThrowsArgumentException()
        {
            // Should throw ArgumentException when input doesn't end with '#'
            Assert.Throws<ArgumentException>(() => OldPhonePadSimulator.OldPhonePad("222"));
        }

        #endregion
    }
}
