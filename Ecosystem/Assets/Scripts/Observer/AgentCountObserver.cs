using TMPro;
using UnityEngine;

/// <summary>
/// Observes the count of different types of agents and updates the UI accordingly.
/// </summary>
public class AgentCountObserver : MonoBehaviour {
    [SerializeField] private TMP_Text rabbitCountText;
    [SerializeField] private TMP_Text foxCountText;
    [SerializeField] private TMP_Text deerCountText;
    [SerializeField] private TMP_Text bearCountText;

    private int m_rabbitCount;
    private int m_foxCount;
    private int m_deerCount;
    private int m_bearCount;

    private void Start() {
        AgentFactory.s_instance.onRabbitBorn += rabbitBorn;
        AgentFactory.s_instance.onRabbitDead += rabbitDead;
        AgentFactory.s_instance.onFoxBorn += foxBorn;
        AgentFactory.s_instance.onFoxDead += foxDead;
        AgentFactory.s_instance.onDeerBorn += deerBorn;
        AgentFactory.s_instance.onDeerDead += deerDead;
        AgentFactory.s_instance.onBearBorn += bearBorn;
        AgentFactory.s_instance.onBearDead += bearDead;
    }

    private void OnDestroy() {
        AgentFactory.s_instance.onRabbitBorn -= rabbitBorn;
        AgentFactory.s_instance.onRabbitDead -= rabbitDead;
        AgentFactory.s_instance.onFoxBorn -= foxBorn;
        AgentFactory.s_instance.onFoxDead -= foxDead;
        AgentFactory.s_instance.onDeerBorn -= deerBorn;
        AgentFactory.s_instance.onDeerDead -= deerDead;
        AgentFactory.s_instance.onBearBorn -= bearBorn;
        AgentFactory.s_instance.onBearDead -= bearDead;
    }

    private void rabbitBorn() {
        m_rabbitCount++;
        setText();
    }

    private void rabbitDead() {
        m_rabbitCount--;
        setText();
    }

    private void foxBorn() {
        m_foxCount++;
        setText();
    }

    private void foxDead() {
        m_foxCount--;
        setText();
    }

    private void deerBorn() {
        m_deerCount++;
        setText();
    }

    private void deerDead() {
        m_deerCount--;
        setText();
    }

    private void bearBorn() {
        m_bearCount++;
        setText();
    }

    private void bearDead() {
        m_bearCount--;
        setText();
    }

    private void setText() {
        rabbitCountText.text = $"Rabbits: {m_rabbitCount}";
        foxCountText.text = $"Foxes: {m_foxCount}";
        deerCountText.text = $"Deers: {m_deerCount}";
        bearCountText.text = $"Bears: {m_bearCount}";
    }
}