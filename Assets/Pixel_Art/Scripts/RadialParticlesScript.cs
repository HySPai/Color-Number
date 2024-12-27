/*
http://www.cgsoso.com/forum-211-1.html

CG搜搜 Unity3d 每日Unity3d插件免费更新 更有VIP资源！

CGSOSO 主打游戏开发，影视设计等CG资源素材。

插件由会员免费分享，如果商用，请务必联系原著购买授权！

daily assets update for try.

U should buy a license from author if u use it in your project!
*/

using UnityEngine;

public class RadialParticlesScript : MonoBehaviour
{
    public int numParticles = 8;

    public float maxRadius = 2f;

    public float processLifetime = 1f;

    public float particleSize = 1f;

    public int loops = 3;

    private float currDegrees;

    private float curveRate;

    private float currRadius;

    private int currLoop;

    private bool deactivateOnComplete = true;

    private ParticleSystem.Particle[] particles;

    private void Start()
    {
        this.particles = new ParticleSystem.Particle[this.numParticles];
        this.curveRate = 90f / this.processLifetime;
        this.currLoop = this.loops;
        this.Reset(this.deactivateOnComplete);
        base.gameObject.SetActive(false);
    }

    private void Update()
    {
        this.currDegrees += this.curveRate * Time.smoothDeltaTime;
        this.currRadius = Mathf.Sin(0.0174532924f * this.currDegrees) * this.maxRadius;
        this.currRadius = Mathf.Clamp(this.currRadius * 1.5f, 0f, this.maxRadius);
        this.UpdateParticles(this.currRadius);
        if (this.currDegrees > 90f)
        {
            this.currLoop--;
            if (this.currLoop > 0)
            {
                this.Reset(this.deactivateOnComplete);
            }
            else if (this.deactivateOnComplete)
            {
                base.gameObject.SetActive(false);
            }
            else
            {
                base.enabled = false;
            }
        }
    }

    public void Reset(bool deactivateGO)
    {
        this.deactivateOnComplete = deactivateGO;
        this.currDegrees = 0f;
        this.currRadius = Mathf.Sin(0.0174532924f * this.currDegrees) * this.maxRadius;
        for (int i = 0; i < this.numParticles; i++)
        {
            ParticleSystem.Particle val = this.particles[i];
            Vector3 position = base.transform.position;
            val.position = new Vector3(0f, 0f, position.z);
            this.particles[i].color = new Color(1f, 1f, 1f, 1f);
            this.particles[i].size = this.particleSize;
            this.particles[i].remainingLifetime = this.processLifetime;
            this.particles[i].startLifetime = this.processLifetime;
        }
        base.GetComponent<ParticleSystem>().SetParticles(this.particles, this.particles.Length);
        base.gameObject.SetActive(true);
    }

    private void UpdateParticles(float radius)
    {
        float a = 1f;
        float num = 0f;
        float num2 = 6.28318548f / (float)this.numParticles;
        for (int i = 0; i < this.numParticles; i++)
        {
            float num3 = Mathf.Cos(num2 * (float)i) * radius;
            num = Mathf.Sin(num2 * (float)i) * radius;
            ParticleSystem.Particle val = this.particles[i];
            float x = num3;
            float y = num;
            Vector3 position = base.transform.position;
            val.position = new Vector3(x, y, position.z);
            if (this.particles[i].remainingLifetime < this.processLifetime * 0.5f)
            {
                a = this.particles[i].remainingLifetime / this.processLifetime;
            }
            this.particles[i].color = new Color(1f, 1f, 1f, a);
            this.particles[i].size = this.particleSize;
            this.particles[i].remainingLifetime -= Time.smoothDeltaTime;
        }
        base.GetComponent<ParticleSystem>().SetParticles(this.particles, this.particles.Length);
    }
}


