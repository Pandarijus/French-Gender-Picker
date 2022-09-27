using UnityEngine;

public class WordData: ScriptableObject
{
  public string word;
  public bool isMasculine;

  public override string ToString()
  {
    string gender = (isMasculine) ? "masculine" : "feminine";
    return   $"\"{word}\" is {gender}";
  }
}
