using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GeoCollect : MonoBehaviour
{
    [SerializeField] private Animator collectAni;
    [SerializeField] private AudioClip[] geoCollects;
    private AudioSource geoCollectSource;
    [SerializeField] private int geoCount = 0;
    [SerializeField] private Text geoText;

    private void Start()
    {
        geoCollectSource= GetComponent<AudioSource>();
        geoText.text = geoCount.ToString();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Geo"))
        {
            collectAni.SetTrigger("Collect");
            int index = Random.Range(0, geoCollects.Length);
            geoCollectSource.PlayOneShot(geoCollects[index]);
            geoCount++;
            geoText.text = geoCount.ToString();
            Destroy(collision.gameObject);
        }
    }
}
