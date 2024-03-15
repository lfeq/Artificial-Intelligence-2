using TMPro;
using UnityEngine;

public class AgentCountObserver : MonoBehaviour {
    [SerializeField] private TMP_Text rabbitCountText;
    [SerializeField] private TMP_Text foxCountText;
    [SerializeField] private TMP_Text deerCountText;
    [SerializeField] private TMP_Text bearCountText;

    private int rabbitCount;
    private int foxCount;
    private int deerCount;
    private int bearCount;

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
        rabbitCount++;
        setText();
    }

    private void rabbitDead() {
        rabbitCount--;
        setText();
    }

    private void foxBorn() {
        foxCount++;
        setText();
    }

    private void foxDead() {
        foxCount--;
        setText();
    }

    private void deerBorn() {
        deerCount++;
        setText();
    }

    private void deerDead() {
        deerCount--;
        setText();
    }

    private void bearBorn() {
        bearCount++;
        setText();
    }

    private void bearDead() {
        bearCount--;
        setText();
    }

    private void setText() {
        rabbitCountText.text = $"Rabbits: {rabbitCount}";
        foxCountText.text = $"Foxes: {foxCount}";
        deerCountText.text = $"Deers: {deerCount}";
        bearCountText.text = $"Bears: {bearCount}";
    }
}