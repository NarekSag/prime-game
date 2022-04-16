using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpController : MonoBehaviour
{
    private const string PLAYER = "Player";

    [SerializeField] private GameObject prime;
    [SerializeField] private ParticleSystem particle;
    [SerializeField] private SOPowerUp[] powerUps;

    private SOPowerUp selectedPowerUp; 

    private void Start()
    {
        selectedPowerUp = GetRandomPowerUp();
        prime.GetComponent<MeshRenderer>().material.mainTexture = selectedPowerUp.actorTexture;
        particle.transform.localScale = Vector3.one * 2;
        ParticleSystem.MainModule mainModule = particle.main;
        mainModule.startColor = selectedPowerUp.particleColor;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals(PLAYER))
        {
            SetPowerUpAbility(selectedPowerUp.powerUpType);
            //TO DO: Animate the {selectedPowerUp.particleColor} alpha as well
            string powerUpName = $"<color=#{ColorUtility.ToHtmlStringRGBA(selectedPowerUp.particleColor)}>{selectedPowerUp.actorName.ToUpper()}</color>";
            //string output = string.Format("You have picked <color=#" + ColorUtility.ToHtmlStringRGBA(selectedPowerUp.particleColor) + ">{0}</color>", selectedPowerUp.actorName.ToUpper());
            GameController.instance.ToastMessage.ShowToastMessage($"You have picked {powerUpName}");
            Destroy(this.gameObject);
        }
    }

    private SOPowerUp GetRandomPowerUp()
    {
        return powerUps[Random.Range(0, powerUps.Length)];
    }

    private void SetPowerUpAbility(SOPowerUp.PowerUpType powerUpType)
    {
        if(powerUpType.Equals(SOPowerUp.PowerUpType.plusHealth))
        {
            GameController.instance.increaseHealthEvent.Invoke();
        }
        else if (powerUpType.Equals(SOPowerUp.PowerUpType.shield))
        {
            GameController.instance.playerController.SetShieldBubbleState(true);
        }
        else if (powerUpType.Equals(SOPowerUp.PowerUpType.magnet))
        {
            GameController.instance.playerController.SetMagnetState(true);
        }
        else if (powerUpType.Equals(SOPowerUp.PowerUpType.doubleMoney))
        {
            GameController.instance.SetDoubleCurrency();
        }
        else if (powerUpType.Equals(SOPowerUp.PowerUpType.doubleScore))
        {
            GameController.instance.SetDoubleScore();
        }
    }
}
