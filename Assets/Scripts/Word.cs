
using System;
using System.Collections;
using System.Collections.Generic;

public class Word
{
    public string word;
    public float maxTime;
    public ArrayList wordLeft;
    public Word(string word)
    {
        this.word = word;
        maxTime = word.Length * 0.4f;
        wordLeft = new ArrayList(word.ToCharArray());
    }

    public char getChar()
    {
        return (char) wordLeft[0];
    }

    public void removeChar()
    {
        wordLeft.RemoveAt(0);
        string newWord = "";
        foreach (var x in wordLeft)
        {
            newWord += x;
        }

        word = newWord;
    }

    public bool wordCompleted()
    {
        if (wordLeft.Count > 0)
            return false;
        return false;
    }
}
