using System;
using System.Collections.Generic;


class Rule
{
    public char S;
    private string w_right;

    public Rule(char s, string st){
        S = s;
        w_right = st;
    }

    public bool Check(char c){
        for (int i = 0; i < w_right.Length; i++)
            if (c == w_right[i])
                return true;
        return false;
    }

    public bool Check(Rule X, Rule Y){
        if (w_right[0] == X.S && w_right[1] == Y.S)
            return true;
        return false;
    }
}



class CYK
{
    List<Rule> G;
    string word;
    char StartSymbol;
    bool [,,]T;
    bool result;
    int n, k;

    public CYK(string w, List<Rule> g, char s = 'S'){
        word = w;
        n = word.Length;
        G = g;
        k = G.Count;
        StartSymbol = s;
        T = new bool[n, n, k];
    }



    private void InitTable(){
        for (int i = 0; i < n; i++){
            for (int j = 0; j < k; j++){
                if (G[j].Check(word[i])){
                    T[i, 0, j] = true;
                }
            }
        }
    }


    private void SetResult(){
        for (int i = 0; i < k; i++){
            if (T[0, n - 1, i] && G[i].S == StartSymbol){
                result = true;
                break;
            }
        }
    }

    public void Parse(){
        int i, j, k, x, y, z;

        InitTable();

        for (i = 1; i < n; i++)
            for (j = 0; j < n - i; j++)
                for (k = 0; k < i; k++)
                    for (x = 0; x < k; x++)
                        for (y = 0; y < k; y++)
                            if (T[j, k, x] && T[j + k + 1, i - k - 1, y])
                                for (z = 0; z < k; z++)
                                    if (G[z].Check(G[x], G[y]))
                                        T[j, i, z] = true;

        SetResult();
    }

    public void PrintTable(){
        for (int i = n - 1; i >= 0; i--){
            for (int j = 0; j < n; j++){
                Console.Write('|');

                for (int z = 0; z < k; z++){
                    if (T[j, i, z]){
                        Console.Write(G[z].S);
                        Console.Write(z+1);
                    }
                    else
                        Console.Write("  ");
                }
                if (j == n - 1)
                    Console.Write('|');
            }
            Console.WriteLine();
        }
        Console.WriteLine();
    }


    public bool GetResult(){
        return result;
    }

}


class Program
{
    static void Main(string[] args){
        List<Rule> G = new List<Rule>();

        G.Add(new Rule('S', "AS"));
        G.Add(new Rule('A', "SA"));
        G.Add(new Rule('S', "b"));
        G.Add(new Rule('A', "a"));

        string word = "abab";

        CYK parser = new CYK(word, G);

        parser.Parse();
        parser.PrintTable();


        if (!parser.GetResult()){
            Console.WriteLine("Failed");
        }
        Console.ReadLine();
    }
}
