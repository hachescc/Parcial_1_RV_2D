using UnityEngine;

public class AsignadorDeRoles : MonoBehaviour
{
    public RolJugador Rol { get; private set; }
    public string NombreJugador { get; private set; }
    public string NombreColor { get; private set; }
    public string DescripcionControles { get; private set; }

    public string NombreRol => InfoRoles.NombreRol(Rol);
    public string DescripcionRol => InfoRoles.DescripcionRol(Rol);

    public void Configurar(string nombreJugador, string nombreColor, string descripcionControles, RolJugador rol)
    {
        NombreJugador = nombreJugador;
        NombreColor = nombreColor;
        DescripcionControles = descripcionControles;
        Rol = rol;
        Debug.Log($"[AsignadorDeRoles] {nombreJugador} ({nombreColor}, {descripcionControles}) es {InfoRoles.NombreRol(rol)}.");
    }
}
