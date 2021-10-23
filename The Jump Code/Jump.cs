using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jump : MonoBehaviour
{
    // ประกาศค่า ว่าเราจะเรียกของส่งนั้น หรือ ข้อมูลนั้น ๆ ว่าอะไร 
    public Rigidbody Ijump;
    public GameObject coinPrefab;
    public GameObject coinRespawningPoint;
    public AudioSource coinSound;
    public AudioSource timeupSound;
    public AudioSource bgSound;
    public AudioSource fallSound;
    public TMPro.TextMeshProUGUI Timeouttext;
    public TMPro.TextMeshProUGUI timeText;
    public TMPro.TextMeshProUGUI txtScore;
    public ParticleSystem SpawnSystem;
    public ParticleSystem CoinSpawnm;
    public bool timerIsRunning = false;
    public float minHeightForDeath = -5;
    public float TimeRemaining;
    public int score;
    // Start is called before the first frame update
    void Start()
    {
        Game_Start_Setting();
    }

    public void Game_Start_Setting(){
        score = 0;
        txtScore.text = "Score : " + score.ToString();
        TimeRemaining = 30;
        bgSound.Play();
        SpawnSystem.Stop();
        timeText.enabled = true;
        txtScore.enabled = true;
        Timeouttext.enabled = false;
        timerIsRunning = true;
        for (int i = 0; i <= 5;i++){
            var position = new Vector3(Random.Range(-20.0f,20.0f), 3, Random.Range(-20.0f, 20.0f));
            CoinSpawnm.transform.position = position;
            Instantiate(coinPrefab, position, coinRespawningPoint.transform.rotation);
        }

    }
    //สร้าง function ว่าถ้าตัวละครชนเหรียญให้ทำอะไรบ้าง
    private void OnCollisionEnter(Collision collision) {
        if (collision.gameObject.tag == "Coin"){
            coinSound.Play();
            CoinSpawnm.Stop();

            Destroy(collision.gameObject);
            score++; 

            StartCoroutine(RespawnCoin());
        }
    }

    
    // Update is called once per frame << frame 
    void Update()
    {
        //แสดงข้อความคะแนนบนหน้าจอ
        txtScore.text = "Score : " + score.ToString();
        //Timer Area 
        //เช็คการเดินของเวลา 
        if(timerIsRunning){
        //เช็คว่าตอนนี้เวลาตอนนี้มีค่าเท่าไหร่ 
        if (TimeRemaining > 0){
            //นับถอยหลัง
            TimeRemaining -= Time.deltaTime;
            //แสดงผลเวลาที่นับถอยหลังบนหน้าจอ
            DisplayTime(TimeRemaining);
            //กดแต่ละ Key แล้วเกิดอะไรบ้าง
            if(Input.GetKeyDown(KeyCode.A)){
            Ijump.AddForce(new Vector3(-20f,0f,0f),ForceMode.Impulse);
            }
            if(Input.GetKeyDown(KeyCode.D)){
            Ijump.AddForce(new Vector3(20f,0f,0f),ForceMode.Impulse);

            }
            if(Input.GetKeyDown(KeyCode.W)){
            Ijump.AddForce(new Vector3(0f,0f,20f),ForceMode.Impulse);

            }
            if(Input.GetKeyDown(KeyCode.S)){
            Ijump.AddForce(new Vector3(0f,0f,-20f),ForceMode.Impulse);

            }
            if(Input.GetKeyDown(KeyCode.Space)){
            Ijump.AddForce(new Vector3(0f,20f,0f),ForceMode.Impulse);

            }
            //ถ้าตัวละครตก Map จะเกิดอะไรขึ้น
            if(Ijump.transform.position.y <= minHeightForDeath){
                    //เล่นเสียง
                    fallSound.Play();
                    //สร้าง Position ใหม่ของตัวละคร
                    Ijump.transform.position = new Vector3(0, 1, 0);
                    //สร้าง Particle ตอนที่ตัวละครเกิดใหม่ และ ให้เล่นแค่ 5 (ไม่ชั่วร์ว่าค่าอะไร แต่น่าจะเป็นวินาที)
                    SpawnSystem.Play();
                    SpawnSystem.time = 5;
            }
        }
        else
        {
            //ถ้าเวลาหมดแล้วเกืดอะไรขึ้น
            timeup_sound();

        }
        }
        else{
            //ถ้ากด Spacebar ค่าทุกอย่างจะย้อนกลับไปที่จุดเริ่มต้น
            if(Input.GetKey(KeyCode.Space)){
                Game_Start_Setting();
            }

        }
        
    }
    //Method สำหรับตอนหมดเวลา ว่าเมื่อหมดเวลาให้ทำอะไรบ้าง
    void timeup_sound(){
        //หยุดการเดินของเวลาเกมส์ เพื่อไม่ให้เวลาเป็นติดลบ
        timerIsRunning = false;
        //หยุดเล่น BG sound
        bgSound.Stop();
        //แสดงข้อความบนหน้าจอว่า เวลาหมดแล้ว และคะแนนที่ทำได้เท่าไหร่
        Timeouttext.text = "Time has run out! \n your score : " + score.ToString();     
        // เปิดการแสดงผลของเวลาที่หมดแล้วบนหน้าจอ
        Timeouttext.enabled = true;
        // เล่นเสียงตอนหมดเวลา
        timeupSound.Play();
        // ปิดการแสดงข้อความของเวลา และ คะแนน เพื่อความสวยงาม
        timeText.enabled = false;
        txtScore.enabled = false;
    }
    // Method การเกิดใหม่ของเหรียญ 
    private IEnumerator RespawnCoin(){
        yield return new WaitForSeconds(3f);
        var position = new Vector3(Random.Range(-20.0f,20.0f), 3, Random.Range(-20.0f, 20.0f));
        CoinSpawnm.transform.position = position;
        CoinSpawnm.Play();
        CoinSpawnm.time = 5;
        Instantiate(coinPrefab, position, coinRespawningPoint.transform.rotation);
    }
    //function ไว้แสดงเวลาที่หน้าจอ 
    void DisplayTime(float timeToDisplay)
    {
        float minutes = Mathf.FloorToInt(timeToDisplay / 60); 
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);  
        timeText.text = string.Format("Time : {0:00}:{1:00}", minutes, seconds);                                                   
    }
    
    



}
