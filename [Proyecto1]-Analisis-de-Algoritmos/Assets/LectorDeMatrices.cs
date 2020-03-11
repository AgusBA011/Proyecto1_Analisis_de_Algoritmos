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
        Matriz = new int[columns, rows];

        for (int i = 0; i < columns; i++) {
            for (int j = 0; j < rows; j++) {
                Matriz[i, j] = 0;
                SpawnTile(i, -j, Matriz[i, j]);
                
            }
        }



    }

    //Función que resuelve el nonogram utilizando backtracking
    bool resolverNono(int [,] Matriz) {

        //Revisa si quedan espacios sin marcar ya ea un color o una X
        if (emptySpace(Matriz) == false) {
            return true;
        }

        //Inicio del ciclo que irá celda por celda de la matriz comprobando si sive o no
        for (int i = 0; i < rows; i++){
            for (int j = 0; j < columns; j++){
                if (isSafe(Matriz, i, j) == true) {  //Hace un checkeo si es posible poner color en la celda dada

                    Matriz[i, j] = 1;
                    if (resolverNono(Matriz) == true) 
                    //Llamada recursiva, hecho el cambio anterior avanza suponiendo que sí sirve. Se devuelve si no sirve.
                    {
                        return true;
                    }
                    //Si no sirve la única otra opción es que no sea ponerle color
                    else {
                        Matriz[i, j] = -1;
                    }
                }
                else { //Si el isSafe da falso significa que es imposible poner colo en esa celda, por ende va con una X
                    Matriz[i, j] = -1;
                }
            }
        }
        return false;

    }
    //Función que revisa si es posible colorear esa celda o no
    bool isSafe(int [,] Matriz, int i, int j){

        if (isSafeR(Matriz, i, j) == true && isSafeC(Matriz, i, j) == true) return true;
        return false;

    }


    bool isSafeR(int[,] Matriz, int i, int j) {

        //Checkeo de la fila
        int counter = 0;
        int slot = 0;
        if (j == 0) return true;
        foreach (string pista in pistasFilas[i])
        {

            while (slot != j)
            {

                if (Matriz[i, slot] == 1) counter++;

                if (Matriz[i, slot] == -1) counter = 0;

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
        return false;
    }

    bool isSafeC(int[,] Matriz, int i, int j)
    {

        //Checkeo de la fila
        int counter = 0;
        int slot = 0;
        if (i == 0) return true;
        foreach (string pista in pistasColumnas[i])
        {

            while (slot != i)
            {

                if (Matriz[slot, j] == 1) counter++;

                if (Matriz[slot, j] == -1) counter = 0;

                if (int.Parse(pista) == counter)
                {
                    if (slot + 1 == i) return false;
                    slot++;
                    break;
                }
                slot++;

                if (slot == i) return true;
            }
        }
        return false;
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
                    Debug.Log(aux[i]);
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
                        Debug.Log("Estoy agregando para filas:" + i);
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
                        Debug.Log("Estoy agregando para columnas:" + i);
                    }
                    pistasColumnas.Add(aux2);
                }
            }

        }
        //TESTING
        for (int e = 0; e < pistasFilas.Count; e++)
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
        }
    }


    

    // Update is called once per frame
    void Update()
    {
        
    }
}
