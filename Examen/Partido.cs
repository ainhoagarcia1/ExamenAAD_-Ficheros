public class Partido
{
    public string jornada;
    public string equipo_local;
    public string equipo_visitante;
    public string resultado;
    public string mvp_partido;
    public string puntos_mvp;
    public string asistencias_mvp;
    public string rebotes_mvp;
    public string equipo_mvp;


    //CONSTRUCTOR
    public Partido(string jornada, string equipo_local, string equipo_visitante, string resultado, string mvp_partido, string puntos_mvp, string asistencias_mvp, string rebotes_mvp, string equipo_mvp)
    {
        this.jornada = jornada;
        this.equipo_local = equipo_local;
        this.equipo_visitante = equipo_visitante;
        this.resultado = resultado;
        this.mvp_partido = mvp_partido;
        this.puntos_mvp = puntos_mvp;
        this.asistencias_mvp = asistencias_mvp;
        this.rebotes_mvp = rebotes_mvp;
        this.equipo_mvp = equipo_mvp;
    }

    public override string ToString ()
    {
        return  this.jornada + ": " +  this.equipo_local + " - " +  this.equipo_visitante + " (" +  this.resultado + "). " +
        "\n     MVP: " +  this.mvp_partido + " ("+ this.equipo_mvp + ") || PTS: " + this.puntos_mvp + " | AST: " + this.asistencias_mvp + " | REB: " + this.rebotes_mvp+"\n";

    }

    public int Ganador()
    {
        string[] resultado = this.resultado.Split("-");
        int diferencia = int.Parse(resultado[0])-int.Parse(resultado[1]);
        return diferencia;
    }
}