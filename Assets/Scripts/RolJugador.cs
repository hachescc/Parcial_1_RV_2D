using UnityEngine;

public enum RolJugador
{
    Ingeniero,
    Medico,
    Piloto,
    Comunicaciones
}

// Textos y colores de cada rol, centralizados acá para no repetirlos
// en AsignadorDeRoles, PanelDeRol y AnuncioDeRoles.
public static class InfoRoles
{
    public static string NombreRol(RolJugador rol)
    {
        switch (rol)
        {
            case RolJugador.Ingeniero: return "Ingeniero";
            case RolJugador.Medico: return "Médico";
            case RolJugador.Piloto: return "Piloto";
            case RolJugador.Comunicaciones: return "Oficial de Comunicaciones";
            default: return rol.ToString();
        }
    }

    // Frase de personalidad + por qué es indispensable. Se muestra en el anuncio de inicio de nivel.
    public static string DescripcionRol(RolJugador rol)
    {
        switch (rol)
        {
            case RolJugador.Ingeniero:
                return "Conoce cada cable y cada tubería de la estación. Solo él puede forzar los paneles técnicos y las compuertas de emergencia.";
            case RolJugador.Medico:
                return "El único capaz de estabilizar el soporte vital. Sin él, la estación se queda sin oxígeno.";
            case RolJugador.Piloto:
                return "El único autorizado para operar la consola de navegación y trazar la ruta de escape.";
            case RolJugador.Comunicaciones:
                return "El único que puede decodificar las señales de auxilio y desbloquear los accesos de seguridad.";
            default:
                return "";
        }
    }

    // Color del panel que corresponde a cada rol (para pintar el PanelDeRol en el Editor).
    public static Color ColorPanel(RolJugador rol)
    {
        switch (rol)
        {
            case RolJugador.Ingeniero: return new Color(1f, 0.55f, 0.1f);       // naranja
            case RolJugador.Medico: return new Color(0.2f, 0.85f, 0.35f);       // verde
            case RolJugador.Piloto: return new Color(0.25f, 0.55f, 1f);        // azul
            case RolJugador.Comunicaciones: return new Color(0.7f, 0.3f, 0.9f); // púrpura
            default: return Color.white;
        }
    }
}
