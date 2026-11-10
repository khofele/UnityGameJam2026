public abstract class Option// : MonoBehaviour
{
    public enum Zustand { Gestartet, Gestoppt, Pausiert }
    public Zustand CurrentZustand = Zustand.Gestartet;

    protected float m_dauer;
    protected float m_cooldown;

    protected Zustand m_zustandVorher = Zustand.Gestartet;

    private bool m_bReady = true;

    //protected abstract void OnPause(); 
    protected abstract void OnTick(float fTimeDelta);

    public void Starten(float dauer = -1f, float cooldown = -1f)
    {
        if (!m_bReady) return;
        if (CurrentZustand == Zustand.Gestartet) return;

        m_dauer = dauer;
        m_cooldown = cooldown;
        if (cooldown > 0f) m_bReady = false;

        //OnStart();

        m_zustandVorher = CurrentZustand;
        CurrentZustand = Zustand.Gestartet;
    }
    public void Stoppen(float dauer = -1f, float cooldown = -1f)
    {
        if (!m_bReady) return;
        if (CurrentZustand == Zustand.Gestoppt) return;

        m_dauer = dauer;
        m_cooldown = cooldown;
        if (cooldown > 0f) m_bReady = false;

        //OnStop();

        m_zustandVorher = CurrentZustand;
        CurrentZustand = Zustand.Gestoppt;
    }
    public void Pausieren(float dauer = -1f, float cooldown = -1f)
    {
        if (!m_bReady) return;
        if (CurrentZustand == Zustand.Pausiert) return;

        m_dauer = dauer;
        m_cooldown = cooldown;
        if (cooldown > 0f) m_bReady = false;

        //OnPause();

        m_zustandVorher = CurrentZustand;
        CurrentZustand = Zustand.Pausiert;
    }
    public void Tick(float fTimeDelta)
    {
        if (m_cooldown > 0f)
        {
            m_cooldown -= fTimeDelta;
            if (m_cooldown <= 0f)
            {
                m_bReady = true;
                m_cooldown = 0f;
            }
        }
        if (m_dauer > 0f)
        {
            m_dauer -= fTimeDelta;
            if (m_dauer <= 0f)
            {
                CurrentZustand = m_zustandVorher;
                m_dauer = 0f;
                //OnPause();
            }
        }
        //if (m_zustand == Zustand::Gestartet) {
        OnTick(fTimeDelta);
        //}
    }
}
