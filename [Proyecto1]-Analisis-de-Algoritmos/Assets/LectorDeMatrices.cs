using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class LectorDeMatrices : MonoBehaviour
{
    // Start is called before the first frame update

    void Start()
    {
        StreamReader sr = new StreamReader("C:/Users/Agus/Documents/GitHub/Proyecto1_Analisis_de_Algoritmos/[Proyecto1]-Analisis-de-Algoritmos/Assets/nonogram.txt");
        string line = "";

        string columnas, filas;
        string[] aux;
        short con = 0;

        List<List<string>> pistasFilas = new List<List<string>>();

        List<List<string>> pistasColumnas = new List<List<string>>();

        while ((line = sr.ReadLine()) != null) { //Lee todas las lineas dentro del .txt

            if (con == 0) { //Obtener el num de columnas y filas
                aux = line.Split(',');
                for (int i = 0; i < 2; i++) {
                    if (i == 0) filas = aux[i];
                    if (i == 1) columnas = aux[i];
                    Debug.Log(aux[i]);
                }
                con++;
            }

            if (line.Equals("FILAS") == true) { //Sección del txt donde dan las pistas de las filas
                con++;
            }
            if (line.Equals("COLUMNAS") == true)//Sección del txt donde dan las pistas de las columnas
            {
                con++;
            }

            if (con == 2) { //Obtener las pistas que hay en cada fila
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
        for(int e = 0; e < pistasFilas.Count; e++)
        {
            for (int i = 0; i < pistasFilas[e].Count; i++) {

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
