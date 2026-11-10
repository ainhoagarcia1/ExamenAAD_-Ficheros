int opcion = 0; //si no lo incializamos fuera del do while no deja usarlo 

//LEER CSV:
List<Partido> liga = new List<Partido>();
string[] lines = File.ReadAllLines("acb2526.csv");
for(int i=1; i < lines.Length; i++)
{
    string[] fields = lines[i].Split(',');
    Partido p = new Partido(fields[0], fields[1], fields[2], fields[3], fields[4], fields[5],fields[6],fields[7],fields[8]);
    liga.Add(p);
}

//MENÚ:
do{
    Console.WriteLine("===================");
    Console.WriteLine("        Menú:       ");
    Console.WriteLine("===================");
    Console.WriteLine(" ");
    Console.WriteLine("¿Como quieres ver los datos?: ");
    Console.WriteLine(" ");
    Console.WriteLine("1. Por equipo.");
    Console.WriteLine("2. Por jornada.");
    Console.WriteLine("3. Top 3 MVPs.");
    Console.WriteLine("4. Clasificación.");
    Console.WriteLine("5. Exportar ficheros por equipo.");
    Console.WriteLine("0. Cerrar la aplicación. ");
    bool error = true;
    do
    {
        Console.WriteLine(" ");
        Console.WriteLine("Selecciona una opción:");
        String opt = Console.ReadLine();
        Console.WriteLine(" ");
        try
        {
            opcion = int.Parse(opt);
            error = false;
        }
        catch
        {
            Console.WriteLine(" ");
            Console.WriteLine("ERROR: Escribe un número");
        }    
    }while(error);

    switch (opcion)
    {
        //Por equipo
        case 1: 
            //Pedir equipo
            Console.WriteLine("Escribe el equipo que quieres buscar:");
            string equipo = Console.ReadLine();
            List<Partido> filtradoE = buscarPartidos(equipo);

            //Comprobar si hay datos:
            if(filtradoE.Count == 0)
            {
                Console.WriteLine("No hay datos de ese equipo.");
                break;
            }
            else
            {
                mostarPartidos(filtradoE);
            }
            if (quieresGuardar())
            {
                string ruta = pedirRuta();
                crearTXT(ruta);
                foreach(Partido p in filtradoE){
                    escribirTXT(p.ToString(), ruta);
                }
            Console.WriteLine("Archivo creado correctamente.");
            }
        break;

        //Por jornada
        case 2:
            //Pedir jornada
            Console.WriteLine("Que jornada quieres ver (introduce solo el número). Jornada:");
            string j = Console.ReadLine();
            string jornada = "Jornada " + j.Trim();

            //Buscar los partidos que ha jugado en esa jornada y guardarlos en una lista
            List<Partido> filtradoJ = new List<Partido>();
            foreach(Partido p in liga)
            {
                if(p.jornada == jornada)
                {
                    filtradoJ.Add(p);
                }
            }

            //Comprobar si hay datos:
            if(filtradoJ.Count == 0)
            {
                Console.WriteLine("Jornada no valida.");
                break;
            }
            else
            {
                mostarPartidos(filtradoJ);
            }

            //Guardar:
            if (quieresGuardar())
            {
                string ruta = pedirRuta();
                crearTXT(ruta);
                foreach(Partido p in filtradoJ){
                    escribirTXT(p.ToString(), ruta);
                }
            Console.WriteLine("");
            Console.WriteLine("Archivo creado correctamente.");
            }
        break;
        
        //MVPs
        case 3: 
            List<string> mvp = new List<string>();
            List<int> cant = new List<int>();
            foreach (Partido partido in liga)
            {
                if (mvp.Contains(partido.mvp_partido))
                {
                    int i = mvp.IndexOf(partido.mvp_partido);
                    cant[i]++; 
                }
                else
                {
                    mvp.Add(partido.mvp_partido);
                    cant.Add(1);
                }
            }
            string datos = "";
            for(int i=1; i<=3; i++)
            {
                int indice = cant.IndexOf(cant.Max());
                int veces = cant[indice];
                string nombre = mvp[indice];
                //borramos para que no vuelva a salir:
                cant[indice] = 0;
                string dato = $"{i}. {nombre} con {veces} MVPs";
                datos += "\n"+dato;
                Console.WriteLine(dato);
            }
            Console.WriteLine("");

            if (quieresGuardar())
            {
                string ruta = pedirRuta();
                crearTXT(ruta);
                escribirTXT(datos, ruta);
            Console.WriteLine("Archivo creado correctamente.");
            Console.WriteLine("");
            }
        break;
        //clasificacion
        case 4:
            // Creamos una lista con todos los equipos:
            List<string> equipos = listaEquipos(liga);

            // Los añadimos a la clasificación:
            List<Clasificacion> clasificacion = new List<Clasificacion>();

            foreach (string nombreEquipo in equipos)
            {
                clasificacion.Add(new Clasificacion(nombreEquipo));
            }

            // Recorremos todos los partidos:
            foreach (Partido p in liga)
            {
                // Buscamos los equipos en la clasificación:
                foreach (Clasificacion eq in clasificacion)
                {
                    if (eq.equipo == p.equipo_local)
                    {
                        if (p.Ganador() > 0)
                            eq.aumentarVictorias();
                        else if (p.Ganador() < 0)
                            eq.aumentarDerrotas();
                    }

                    if (eq.equipo == p.equipo_visitante)
                    {
                        if (p.Ganador() < 0)
                            eq.aumentarVictorias();
                        else if (p.Ganador() > 0)
                            eq.aumentarDerrotas();
                    }
                }
            }
            clasificacion.Sort();
            int cont = 1;
            string ranking = "";
            Console.WriteLine("============= CLASIFICACIÓN =============");
            string puesto;
            foreach(Clasificacion eq in clasificacion)
            {
                puesto = $"{cont}. " + eq.ToString();
                ranking += puesto + "\n";
                Console.WriteLine(puesto);
                cont++;
            }
            Console.WriteLine("==================================");   
            Console.WriteLine("");
            Console.WriteLine("");
            if (quieresGuardar())
            {
                string ruta = pedirRuta();
                crearTXT(ruta);
                escribirTXT(ranking, ruta);
            Console.WriteLine("Archivo creado correctamente.");
            Console.WriteLine("");
            }
        break;

        //Exportar txts
        case 5: 
            Console.WriteLine("Los ficheros se guardaran en la carpeta Equipos.");
            //Creamos una lista con todos los equipos:
            List<string> equips = listaEquipos(liga);
            

            //Por cada equipo buscamos sus partidos
            if (Directory.Exists("./Equipos"))
            {
                Console.WriteLine("");
                Console.WriteLine("Se van a sobreescribir los datos de la carpeta Equipos.");
                Console.WriteLine("Introduce 'ok' para continuar:");
                if (Console.ReadLine() != "ok")
                {
                    Console.WriteLine("Acción cancelada");
                    break;
                }
                Console.WriteLine("");
            }
            else
            {
                Directory.CreateDirectory("./Equipos");
                Console.WriteLine("Capeta Equipos creada correctamente");
                Console.WriteLine("");
            }

            foreach(string e in equips)
            {
                List<Partido> partidos = buscarPartidos(e);
                string ruta = Path.Combine("./Equipos", e + ".txt");
                crearTXT(ruta);
                foreach(Partido p in partidos)
                {
                    escribirTXT(p.ToString(), ruta);
                }
            }
            Console.WriteLine($"Se han creado {equips.Count} ficheros en la carpeta Equipos.");
        break;

        //SALIR
        case 0:
            Console.WriteLine("Saliendo del programa...");
            Console.WriteLine(" ");
        break;

        //Otro número
        default:
            
            Console.WriteLine("Selecciona una opcion valida.");
            Console.WriteLine(" ");
        break;

    }
}while(opcion!=0);

List<string> listaEquipos(List<Partido> liga)
{  
    List<string> equipos = new List<string>();
    foreach(Partido p in liga)
    {
        if (!equipos.Contains(p.equipo_local))
        {
            equipos.Add(p.equipo_local);
        }
    }
    return equipos;
}

List<Partido> buscarPartidos(string equipo)
{
    //Buscar los partidos que ha jugado ese equipo y guardarlos en una lista
    List<Partido> filtradoE = new List<Partido>();
    foreach(Partido p in liga)
    {
        if(p.equipo_local == equipo || p.equipo_visitante == equipo)
        {
            filtradoE.Add(p);
        }
    }
    return filtradoE;
}

void mostarPartidos(List<Partido> lista)
{
    foreach(Partido p in lista)
    {
        Console.WriteLine(p.ToString());
    }
}

bool quieresGuardar()
{
    Console.WriteLine("");
    Console.WriteLine("¿Quieres guardar estos datos en un archivo .txt?:");
    Console.WriteLine(" ");
    Console.WriteLine("1. Sí.");
    Console.WriteLine("2. No.");
    
    bool error = true;
    int o = 0;
    do
    {
        Console.WriteLine(" ");
        Console.WriteLine("Selecciona una opción:");
        string opt = Console.ReadLine();
        Console.WriteLine(" ");
        try
        {
            o = int.Parse(opt);
            error = false;
        }
        catch
        {
            Console.WriteLine(" ");
            Console.WriteLine("ERROR: Escribe el número de una de las opciones.");
        }    
    }while(error || !(o==1 || o==2));
    if (o == 1)
    {
        return true;
    }
    return false;
}

string pedirRuta()
{
    Console.WriteLine(" ");
    Console.WriteLine("Escribe el nombre del archivo (sin extensión):");
    return Console.ReadLine()+".txt";
}

void crearTXT(string ruta)
{
    try
    {
        FileStream archivo = File.Create(ruta);
        archivo.Close();
    }catch (Exception ex) {
        Console.WriteLine("Error al crear archivo: " + ex.Message);
    }
}

void escribirTXT(string linea, string ruta)
{
    File.AppendAllText(ruta, linea + Environment.NewLine);
}
    
