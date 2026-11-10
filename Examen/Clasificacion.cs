public class Clasificacion : IComparable<Clasificacion>
{
    public string equipo;
    private int victorias;
    private int derrotas;

    public Clasificacion (String equipo)
    {
        this.equipo = equipo;
        this.victorias = 0;
        this.derrotas = 0;
    }
    
    public void aumentarVictorias()
    {
        this.victorias++;
    }
    public void aumentarDerrotas()
    {
        this.derrotas++;
    }
    
    public override string ToString()
    {
        return this.equipo + " \n        Victorias: " + this.victorias + " | Derrotas: " + this.derrotas;
    }

    public int CompareTo(Clasificacion otroEquipo)
    {
        return otroEquipo.victorias - this.victorias;
    }

}
