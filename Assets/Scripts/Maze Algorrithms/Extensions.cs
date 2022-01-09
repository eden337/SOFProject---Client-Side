using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MazeSide
{
    Center, Left, Right
}

public enum TreatmentType
{
    Bilateral, Smoothness, Midline, Default
}



public enum Pattern
{
    V_CORRIDOR, H_CORRIDOR,
    /*
     *  Vertical Corridor => ||
     *  Horizontal Corridor => =
     */
    H_R_END_PIECE, H_L_END_PIECE, V_T_END_PIECE, V_B_END_PIECE,
    /*
     * Horizontal Right End Piece => =|
     * Horizontal Left End Piece  => |=
     *                              _
     * Vertical Top End Piece =>    |
     * 
     *  Vertical Bottom End Piece =>   |
     *                                 -
     */
    B_R_CORNER, B_L_CORNER, T_R_CORNER, T_L_CORNER,
    /*
     * Bottom Right Corner 
     *                      
     */
    B_TJUNCTION, T_TJUNCTION, R_TJUNCTION, L_TJUNCTION,
    /*
     * Bottom Tjunction 
     * Top Tjunction
     * Right Tjunction -|
     * Left Tjunction |-
     */
    CROSSROAD,
}

public static class PatternExtensions
{
    public static int[] getPattern(this Pattern p)
    {
        switch (p)
        {
            case Pattern.V_CORRIDOR:
                return new int[] { 5, 0, 5, 1, 0, 1, 5, 0, 5 };
            case Pattern.H_CORRIDOR:
                return new int[] { 5, 1, 5, 0, 0, 0, 5, 1, 5 };
            case Pattern.V_T_END_PIECE:
                return new int[] { 5, 1, 5, 1, 0, 1, 5, 0, 5 };
            case Pattern.V_B_END_PIECE:
                return new int[] { 5, 0, 5, 1, 0, 1, 5, 1, 5 };
            case Pattern.H_L_END_PIECE:
                return new int[] { 5, 1, 5, 1, 0, 0, 5, 1, 5 };
            case Pattern.H_R_END_PIECE:
                return new int[] { 5, 1, 5, 0, 0, 1, 5, 1, 5 };

            case Pattern.B_L_CORNER:
                return new int[] { 5, 0, 1, 1, 0, 0, 5, 1, 5 };
            case Pattern.B_R_CORNER:
                return new int[] { 1, 0, 5, 0, 0, 1, 5, 1, 5 };

            case Pattern.T_L_CORNER:
                return new int[] { 5, 1, 5, 1, 0, 0, 5, 0, 1 };
            case Pattern.T_R_CORNER:
                return new int[] { 5, 1, 5, 0, 0, 1, 1, 0, 5 };
            case Pattern.T_TJUNCTION:
                return new int[] { 5, 1, 5, 0, 0, 0, 1, 0, 1 };
            case Pattern.B_TJUNCTION:
                return new int[] { 1, 0, 1, 0, 0, 0, 5, 1, 5 };
            case Pattern.L_TJUNCTION:
                return new int[] { 5, 0, 1, 1, 0, 0, 5, 0, 1 };
            case Pattern.R_TJUNCTION:
                return new int[] { 1, 0, 5, 0, 0, 1, 1, 0, 5 };
            case Pattern.CROSSROAD:
                return new int[] { 1, 0, 1, 0, 0, 0, 1, 0, 1 };
            default:
                return null;
        }
    }
    public static int getBaseType(this Pattern p)
    {
        switch (p)
        {

            case Pattern.V_T_END_PIECE:
            case Pattern.V_B_END_PIECE:
            case Pattern.H_L_END_PIECE:
            case Pattern.H_R_END_PIECE:
                return 0;

            case Pattern.B_L_CORNER:
            case Pattern.B_R_CORNER:
            case Pattern.T_L_CORNER:
            case Pattern.T_R_CORNER:
                return 1;
            case Pattern.T_TJUNCTION:
            case Pattern.B_TJUNCTION:
            case Pattern.L_TJUNCTION:
            case Pattern.R_TJUNCTION:
                return 2;

            case Pattern.CROSSROAD:
                return 3;
            default:
                return -1;
        }
    }
    public static int[] getNeighbors(this Pattern p)
    {

        //0 UP 1 Down 2 Right 3 Left
        switch (p)
        {

            case Pattern.V_T_END_PIECE:
                return new int[] { 1 };
            case Pattern.V_B_END_PIECE:
                return new int[] { 0 };
            case Pattern.H_L_END_PIECE:
                return new int[] { 2 };
            case Pattern.H_R_END_PIECE:
                return new int[] { 3 };

            case Pattern.B_L_CORNER:
                return new int[] { 0, 2 };
            case Pattern.B_R_CORNER:
                return new int[] { 0, 3 };
            case Pattern.T_L_CORNER:
                return new int[] { 1, 2 };
            case Pattern.T_R_CORNER:
                return new int[] { 1, 3 };
            case Pattern.T_TJUNCTION:
                return new int[] { 1, 2, 3 };
            case Pattern.B_TJUNCTION:
                return new int[] { 0, 2, 3 };
            case Pattern.L_TJUNCTION:
                return new int[] { 0, 1, 2 };
            case Pattern.R_TJUNCTION:
                return new int[] { 0, 1, 3 };
            case Pattern.CROSSROAD:
                return new int[] { 0, 1, 2, 3 };
            default:
                return null;
        }
    }
    public static string getMazeType(this Pattern p)
    {
        switch (p)
        {

            case Pattern.V_T_END_PIECE:
                return "V_T_END_PIECE";
            case Pattern.V_B_END_PIECE:
                return "V_B_END_PIECE";
            case Pattern.H_L_END_PIECE:
                return "H_L_END_PIECE";
            case Pattern.H_R_END_PIECE:
                return "H_R_END_PIECE";

            case Pattern.B_L_CORNER:
                return "B_L_CORNER";
            case Pattern.B_R_CORNER:
                return "B_R_CORNER";
            case Pattern.T_L_CORNER:
                return "T_L_CORNER";
            case Pattern.T_R_CORNER:
                return "T_R_CORNER";
            case Pattern.T_TJUNCTION:
                return "T_TJUNCTION";
            case Pattern.B_TJUNCTION:
                return "B_TJUNCTION";
            case Pattern.L_TJUNCTION:
                return "L_TJUNCTION";
            case Pattern.R_TJUNCTION:
                return "R_TJUNCTION";
            case Pattern.CROSSROAD:
                return "CROSSROAD";
            default:
                return "";
        }
    }
    public static Color getMazeTypeColor(this Pattern p)
    {
        switch (p)
        {

            case Pattern.V_T_END_PIECE:
                return Color.yellow;
            case Pattern.V_B_END_PIECE:
                return Color.red;
            case Pattern.H_L_END_PIECE:
                return Color.green;
            case Pattern.H_R_END_PIECE:
                return Color.blue;

            case Pattern.B_L_CORNER:
                return Color.white;
            case Pattern.B_R_CORNER:
                return Color.cyan;
            case Pattern.T_L_CORNER:
                return Color.clear;
            case Pattern.T_R_CORNER:
                return Color.grey;
            case Pattern.T_TJUNCTION:
                return new Color(237, 114, 215);//pink
            case Pattern.B_TJUNCTION:
                return Color.magenta;
            case Pattern.L_TJUNCTION:
                return new Color(171, 36, 148);//purple
            case Pattern.R_TJUNCTION:
                return new Color(252, 186, 3);//orange
            case Pattern.CROSSROAD:
                return new Color(125, 119, 0);//musturd
            default:
                return Color.black;
        }
    }
}
public static class Extensions
{
    private static System.Random rng = new System.Random();
    public static void Shuffle<T>(this IList<T> list)
    {
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = rng.Next(n + 1);
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }


}
