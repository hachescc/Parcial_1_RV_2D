using UnityEngine;

// Va en el Player.prefab. Guarda la identidad de ESTE jugador para la partida actual:
// su color fijo (siempre el mismo, Jugador 1 = Rojo, etc.), sus controles (fijos,
// dependen del esquema que le tocó) y su rol, que PlayerSpawner asigna al azar cada
// vez que arranca el nivel.
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
