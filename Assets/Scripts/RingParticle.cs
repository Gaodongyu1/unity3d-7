using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RingParticle : MonoBehaviour
{

    private ParticleSystem ps; //粒子系统
    private ParticleSystem.Particle[] particles; //粒子系统中的粒子
    public int count = 1000; //粒子数量
    public float size = 0.3f; //粒子大小
    public float maxRadius = 12f;//圆环的最大半径
    public float minRadius = 5f;//圆环的最小半径
    public Gradient gradient; //控制粒子的颜色变换
    private float[] radiuses; //粒子的半径
    private float[] angles; //粒子的角度

    // Start is called before the first frame update
    void Start()
    {
        particles = new ParticleSystem.Particle[count];
        radiuses = new float[count];
        angles = new float[count];
        ps = GetComponent<ParticleSystem>();
        ps.maxParticles = count; 
        ps.loop = false;
        ps.startSpeed = 0; 
        ps.startSize = size; 
        ps.Emit(count); //发射粒子
        ps.GetParticles(particles);
        for (int i = 0; i < count; ++i)
        {    
            float midRadius = (maxRadius + minRadius) / 2;
            float minRate = Random.Range(1.0f, midRadius / minRadius);
            float maxRate = Random.Range(midRadius / maxRadius, 1.0f);
            float radius = Random.Range(minRadius * minRate, maxRadius * maxRate); 
            float angle = Random.Range(0.0f, 360.0f);
            float theta = angle / 180 * Mathf.PI;
            radiuses[i] = radius;
            angles[i] = angle;
            particles[i].position = new Vector3(radius * Mathf.Cos(theta), radius * Mathf.Sin(theta), 0);
        }
        ps.SetParticles(particles, particles.Length);
    }

    // Update is called once per frame
    void Update () 
    {
        for (int i = 0; i < count; ++i)
        {   
            //将粒子的旋转速度分成10个不同的速度，让不同的粒子有不同的速度
            angles[i] -= (i / 100 + 1) / radiuses[i] * Random.Range(1, 3);
            angles[i] = (360.0f + angles[i]) % 360.0f;

            float theta = angles[i] / 180 * Mathf.PI;
            //让粒子的半径在一定范围内波动
            radiuses[i] += Random.Range(-0.01f, 0.01f);
            particles[i].position = new Vector3(radiuses[i] * Mathf.Cos(theta), radiuses[i] * Mathf.Sin(theta), 0);
            //给粒子加上颜色特效
            particles[i].color = gradient.Evaluate(0.5f);
        }
        ps.SetParticles(particles, particles.Length);
    }
}
