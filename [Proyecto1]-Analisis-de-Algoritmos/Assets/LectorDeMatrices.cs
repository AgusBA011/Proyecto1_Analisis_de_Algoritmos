using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.SceneManagement;
using System.Diagnostics;
using UnityEngine.UI;
using System;

public class LectorDeMatrices : MonoBehaviour
{
    // Start is called before the first frame update

    public List<List<string>> pistasFilas = new List<List<string>>();

    public  List<List<string>> pistasColumnas = new List<List<string>>();

    public GameObject[,] cuadros;

    public int rows, columns;
    
    public int[,] Matriz;

    public Sprite spriteEmpty;

    public Sprite spriteFill;

    public Text tiempo;

    public GameObject pistasText;

    private float coordenadasX = -2.83f;
    private float coordenadasY = -2.16f;


    void Start() {

        obtainData();
        Matriz = new int[rows, columns];
        cuadros = new GameObject[rows, columns];

        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < columns; j++)
            {
                Matriz[i, j] = 0;
                SpawnTile(j, -i, Matriz[i, j]);

            }
        }

    }


    public void startSolution() {

        deductionNono(Matriz);
        Stopwatch sw = new Stopwatch();

        sw.Start();

        if (resolverNono(0, 0) == true) imprimirM(Matriz);
        else
        {
            UnityEngine.Debug.Log("No hay solución");
            imprimirM(Matriz);
        }
        sw.Stop();

        Text tm;
       
        tm = tiempo.GetComponent("Text") as Text;
        tm.text = ((double)(sw.Elapsed.TotalMilliseconds * 1000000) /
            1000000).ToString("0.00") + " ms";

        //";
    }

    //Función para volver al menú, se le otorga a un botón.
    public void BackMenu()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
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
        
        if (i == rows) return true;

        Matriz[i, j] = 1;
        GameObject aux = cuadros[i, j];

        aux.GetComponent<SpriteRenderer>().sprite = spriteFill;

        if (isSafe(Matriz, i, j) && resolverNono(i, j + 1)) return true;

        Matriz[i, j] = -1;
        aux.GetComponent<SpriteRenderer>().sprite = spriteEmpty;
        if (isSafe(Matriz, i, j) && resolverNono(i, j + 1)) return true;

        return false;
        

    }
    //Función que revisa si es posible colorear esa celda o no
    bool isSafe(int [,] Matriz, int i, int j){

        return isSafeR(Matriz, i, j) && isSafeC(Matriz, i, j);

    } // Inicio de la función para revisar si un cuadro s ele es seguro pintarlo revisando filas y columnas.

    //Función que revisa si dado un cuadro, este puede ser coloreado o no. PARA LAS FILAS
    bool isSafeR(int[,] Matriz, int i, int j) {
        //Checkeo de la fila

        int k = 0;
        int acc = 0;
        bool isLast = false;
        
        for (int aux = 0; aux <= j; aux++)
        {
            //Debug.Log("Rows");
            //Debug.Log(i.ToString() + aux.ToString() + " : " + Matriz[i, aux].ToString());
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

    //Función que revisa si dado un cuadro, este puede ser coloreado o no. PARA LAS COLUMNAS
    bool isSafeC(int[,] Matriz, int i, int j)
    {

        //Checkeo de la fila

        int k = 0;
        int acc = 0;
        bool isLast = false;
        
        for (int aux = 0; aux <= i; aux++) {
            //Debug.Log("Columns");
            //Debug.Log(aux.ToString() + j.ToString() + " : " + Matriz[aux,j].ToString());
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

    //Imprimir la matriz en consola, esto es para pruebas
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
            UnityEngine.Debug.Log(arrayString);
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


    //Función que deduce solamente utilizando las pistas, si un cuadro debe ir ahí o no. Esto es antes de empezar el bactracking.
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


    //Función para crear el Nonogram en pantalla
    private void SpawnTile(int x, int y, int value) {
        GameObject g = new GameObject("X: " + x + " Y:" + -y);
        g.transform.position = new Vector3(x - 2.5f, y - 2.5f);
        var s = g.AddComponent<SpriteRenderer>();
        s.sprite = spriteEmpty;

        cuadros[-y,x] = g;


        // Agrgar las pistas en pantalla

        TextMesh tm = new TextMesh();
        string pista = "";

       if (y == 0) {
            Instantiate(pistasText);

            float aux_coorY = -1.69f; // Variable que voy a utilizar en caso de que hayan multiples pistas

            
            foreach (string i in pistasColumnas[x]){

                pista = pista + i + System.Environment.NewLine;
                aux_coorY = aux_coorY + 0.6f;
            }

            pistasText.transform.position = new Vector3(coordenadasX, aux_coorY);

            tm = pistasText.GetComponent("TextMesh") as TextMesh;
            tm.text = " " + pista;

            coordenadasX = coordenadasX + 1f;
        }

        pista = "";

        if (x == 0) {
            Instantiate(pistasText);
            ;

            float aux_coorX = -2.9f;

            
            foreach (string i in pistasFilas[-y])
            {                
                pista = pista + i + "  ";
                aux_coorX = aux_coorX - 0.8f;
            }
            tm = pistasText.GetComponent("TextMesh") as TextMesh;
            tm.text = pista;

            pistasText.transform.position = new Vector3(aux_coorX, coordenadasY);

            coordenadasY = coordenadasY + -1f;
        }

     }


    void obtainData() {
        //"C:/Users/Agus/Documents/GitHub/Proyecto1_Analisis_de_Algoritmos/[Proyecto1]-Analisis-de-Algoritmos/Assets/nonogram.txt"

        Main_Menu obj = new Main_Menu();

        if (obj.getNonoGramPath() == "")
        {
            UnityEngine.Debug.Log("Debe elegir un Nonogram");
        }

        else
        {

            StreamReader sr = new StreamReader(obj.getNonoGramPath()); //obj.getNonoGramPath()

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
    }


    

    // Update is called once per frame
    void Update()
    {
        
    }
}
