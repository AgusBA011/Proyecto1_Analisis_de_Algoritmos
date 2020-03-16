using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class LectorDeMatrices : MonoBehaviour
{
    // Start is called before the first frame update

    public List<List<string>> pistasFilas = new List<List<string>>();

    public  List<List<string>> pistasColumnas = new List<List<string>>();

    public int rows, columns;
    
    public int[,] Matriz;

    public Sprite sprite;

    void Start()
    {
        obtainData();
        Matriz = new int[rows, columns];

        for (int i = 0; i < rows; i++) {
            for (int j = 0; j < columns; j++) {
                Matriz[i, j] = 0;
                SpawnTile(j, -i, Matriz[i, j]);
                
            }
        }

        //deductionNono(Matriz);

        if (resolverNono(0, 0) == true) imprimirM(Matriz);
        else
        {
            Debug.Log("verga");
            imprimirM(Matriz);
        }

    }

    //Función que resuelve el nonogram utilizando backtracking
    bool resolverNono(int i, int j) {

        //Revisa si quedan espacios sin marcar ya sea un color o una X
        if (emptySpace(Matriz) == false) {
            return true;
        }
        
        if (j == columns) { 
            i++;
            j = 0;
        }
        Debug.Log("Matriz: " + i.ToString() + " " + j.ToString());
        if (i == rows) return true;

        Matriz[i, j] = 1;

        if (isSafe(Matriz, i, j) && resolverNono(i, j + 1)) return true;

        Matriz[i, j] = -1;
        if (isSafe(Matriz, i, j) && resolverNono(i, j + 1)) return true;

        return false;
        

    }
    //Función que revisa si es posible colorear esa celda o no
    bool isSafe(int [,] Matriz, int i, int j){

        Debug.Log("----------------------------------------------------");
        return isSafeR(Matriz, i, j) && isSafeC(Matriz, i, j);

    } // Arreglar estas funciones
    bool isSafeR(int[,] Matriz, int i, int j) {
        /*
        //Checkeo de la fila
        int counter = 0;
        int slot = 0;
        if (j == 0) return true;
        foreach (string pista in pistasFilas[i])
        {
            while (slot != j)
            {

                if (Matriz[i, slot] == 1) counter++;

                else { counter = 0; }

                if (int.Parse(pista) == counter)
                {
                    if (slot + 1 == j) return false;
                    slot++;
                    break;
                }
                slot++;

                if (slot == j) return true;
            }
        }
        return false;*/



        //Checkeo de la fila

        int k = 0;
        int acc = 0;
        bool isLast = false;
        Debug.Log("New");
        for (int aux = 0; aux <= j; aux++)
        {
            Debug.Log("Rows");
            Debug.Log(i.ToString() + aux.ToString() + " : " + Matriz[i, aux].ToString());
            if (Matriz[i, aux] == 1)
            {
                acc++;
                if (isLast != true)
                {
                    if (k >= pistasFilas[i].Count) return false;
                }
                isLast = true;
            }
            else
            {
                if (isLast == true)
                {

                    if (int.Parse(pistasFilas[i][k]) != acc) return false;
                    acc = 0;
                    k++;
                }
                isLast = false;
            }

        }
        //Revisar si se terminó de revisar la columna
        if (j == columns - 1)
        {

            if (isLast == true)
            {
                return k == pistasFilas[i].Count - 1 && acc == int.Parse(pistasFilas[i][k]);
            }
            else
            {
                return k == pistasFilas[i].Count;
            }

        }
        else
        {
            if (isLast)
            {
                return acc <= int.Parse(pistasFilas[i][k]);
            }
        }
        return true;

    }

    bool isSafeC(int[,] Matriz, int i, int j)
    {

        //Checkeo de la fila

        int k = 0;
        int acc = 0;
        bool isLast = false;
        Debug.Log("New");
        for (int aux = 0; aux <= i; aux++) {
            Debug.Log("Columns");
            Debug.Log(aux.ToString() + j.ToString() + " : " + Matriz[aux,j].ToString());
            if (Matriz[aux, j] == 1)
            {
                acc++;
                if (isLast != true)
                {
                    if (k >= pistasColumnas[j].Count) return false;
                }
                isLast = true;
            }
            else {
                if (isLast == true) {
                    
                    if (int.Parse(pistasColumnas[j][k]) != acc) return false;
                    acc = 0;
                    k++;
                }
                isLast = false;
            }

        }
        //Revisar si se terminó de revisar la columna
        if (i == rows - 1)
        {
            
            if (isLast == true)
            {
                return k == pistasColumnas[j].Count - 1 && acc == int.Parse(pistasColumnas[j][k]);
            }
            else
            {
                return k == pistasColumnas[j].Count;
            }

        }
        else {
            if (isLast) {
                return acc <= int.Parse(pistasColumnas[j][k]);
            }
        }
        return true;
    }


    void imprimirM(int[,] Matriz) {

        int rowLength = Matriz.GetLength(0);
        int colLength = Matriz.GetLength(1);
        string arrayString = "";
        for (int i = 0; i < rowLength; i++)
        {
            for (int j = 0; j < colLength; j++)
            {
                arrayString += string.Format("{0} ", Matriz[i, j]);
            }
            arrayString += System.Environment.NewLine + System.Environment.NewLine;
            Debug.Log(arrayString);
            arrayString = "";
        }

    }

    //Función que revisa todas las celdas y ve si falta una por colorear o poner una X
    bool emptySpace(int[,] Matriz) {

        for (int i = 0; i < rows; i++) {
            for (int j = 0; j < columns; j++) {
                if ( Matriz[i,j] == 0) {
                    return true;
                }
            }
        }
        return false;
    }



    void deductionNono(int[,] Matriz) {

        int a = 0;

        foreach (string pista in pistasFilas[0]) {

            if (pistasFilas.Count == 1) {
                int resta = rows - int.Parse(pista);
                for (int i = resta; i < rows - resta; i++)
                {

                    Matriz[a, i] = 1;
                }
            }
            a++;

        }
        a = 0;
        foreach (string pista in pistasColumnas[0])
        {

            if (pistasColumnas.Count == 1)
            {
                int resta = rows - int.Parse(pista);
                for (int i = resta; i < rows - resta; i++)
                {

                    Matriz[i, a] = 1;
                }
            }
            a++;

        }
    }



























    private void SpawnTile(int x, int y, int value) {
        GameObject g = new GameObject("X: " + x + "Y:" + -y);
        g.transform.position = new Vector3(x - 2.5f, y - 2.5f);
        var s = g.AddComponent<SpriteRenderer>();
        s.sprite = sprite;

     }


    void obtainData() {

        StreamReader sr = new StreamReader("C:/Users/Agus/Documents/GitHub/Proyecto1_Analisis_de_Algoritmos/[Proyecto1]-Analisis-de-Algoritmos/Assets/nonogram.txt");
        string line = "";


        string[] aux;
        short con = 0;



        while ((line = sr.ReadLine()) != null)
        { //Lee todas las lineas dentro del .txt

            if (con == 0)
            { //Obtener el num de columnas y filas
                aux = line.Split(',');
                for (int i = 0; i < 2; i++)
                {
                    if (i == 0) rows = int.Parse(aux[i]);
                    if (i == 1) columns = int.Parse(aux[i]);
                    
                }
                con++;
            }

            if (line.Equals("FILAS") == true)
            { //Sección del txt donde dan las pistas de las filas
                con++;
            }
            if (line.Equals("COLUMNAS") == true)//Sección del txt donde dan las pistas de las columnas
            {
                con++;
            }

            if (con == 2)
            { //Obtener las pistas que hay en cada fila
                if (line.Equals("FILAS") != true)
                {

                    List<string> aux2 = new List<string>();
                    aux = line.Split(',');

                    foreach (string i in aux)
                    {
                        aux2.Add(i);
                        //Debug.Log("Estoy agregando para filas:" + i);
                    }
                    pistasFilas.Add(aux2);
                }

            }

            if (con == 3)
            { //Obtener las pistas que hay en cada columna
                if (line.Equals("COLUMNAS") != true)
                {
                    List<string> aux2 = new List<string>();
                    aux = line.Split(',');
                    foreach (string i in aux)
                    {
                        aux2.Add(i);
                        //Debug.Log("Estoy agregando para columnas:" + i);
                    }
                    pistasColumnas.Add(aux2);
                }
            }

        }
        //TESTING
        /*for (int e = 0; e < pistasFilas.Count; e++)
        {
            for (int i = 0; i < pistasFilas[e].Count; i++)
            {

                Debug.Log("Pistas de filas:" + pistasFilas[e][i]);
            }
        }
        for (int e = 0; e < pistasColumnas.Count; e++)
        {
            for (int i = 0; i < pistasColumnas[e].Count; i++)
            {

                Debug.Log("Pistas de columnas:" + pistasColumnas[e][i]);
            }
        }*/
    }


    

    // Update is called once per frame
    void Update()
    {
        
    }
}
