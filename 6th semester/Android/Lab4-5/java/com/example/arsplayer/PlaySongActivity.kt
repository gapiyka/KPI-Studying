package com.example.arsplayer

import android.graphics.Bitmap
import android.graphics.BitmapFactory
import android.media.AudioAttributes
import android.media.AudioManager
import android.media.MediaMetadataRetriever
import android.media.MediaPlayer
import android.net.Uri
import androidx.appcompat.app.AppCompatActivity
import android.os.Bundle
import android.os.Handler
import android.widget.Button
import android.widget.ImageView
import android.widget.SeekBar
import android.widget.TextView
import java.net.URL


class PlaySongActivity : AppCompatActivity() {

    private lateinit var mp: MediaPlayer
    private val mHandler = Handler()

    private lateinit var positionBar: SeekBar
    private lateinit var elapsedTimeLabel: TextView
    private lateinit var remainingTimeLabel: TextView
    private lateinit var playBtn: Button
    private lateinit var image: ImageView
    private var isLink: Boolean = false

    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        setContentView(R.layout.activity_play_song)

        val audioFile = intent.getParcelableExtra<AudioFile>("audioFile")
        val title = findViewById<TextView>(R.id.title)
        title.text = audioFile!!.displayName

        mp = MediaPlayer()
        mp.reset()
        mp.setAudioStreamType(AudioManager.STREAM_MUSIC)
        isLink = audioFile.displayName.contains("https://")
        if (isLink)
            mp.setDataSource(audioFile.displayName)
        else
            mp.setDataSource(applicationContext, audioFile.uri)
        //mp.setDataSource("https://www.soundhelix.com/examples/mp3/SoundHelix-Song-1.mp3")
        mp.prepare()
        mp.start()
        mp.setVolume(0.5f, 0.5f)
        mp.isLooping = true

        setUIElements(audioFile.uri)
    }

    private fun setUIElements(uri: Uri) {
        positionBar = findViewById(R.id.positionBar)
        elapsedTimeLabel = findViewById(R.id.elapsedTimeLabel)
        remainingTimeLabel = findViewById(R.id.remainingTimeLabel)
        playBtn = findViewById(R.id.playBtn)
        image = findViewById(R.id.audioImage)

        if(!isLink)
            setSongPreview(uri)
        setPlayBtn()
        setPositionBar()
        setVolumeBar()
        updatePositionBar()
    }

    override fun onBackPressed() {
        super.onBackPressed()
        mp.stop()
    }

    private fun updatePositionBar() {
        mHandler.postDelayed(mUpdatePositionBar, 100)
    }

    private val mUpdatePositionBar: Runnable = object : Runnable {
        override fun run() {
            val currentPosition = mp.currentPosition
            positionBar.progress = currentPosition
            elapsedTimeLabel.text = createTimeLabel(currentPosition)
            val remainingTime = createTimeLabel(mp.duration - currentPosition)
            remainingTimeLabel.text = "-$remainingTime"

            mHandler.postDelayed(this, 100)
        }
    }

    fun createTimeLabel(time: Int): String {
        val min = time / 1000 / 60
        val sec = time / 1000 % 60

        var timeLabel = "$min:"
        if (sec < 10) timeLabel += "0"
        timeLabel += sec

        return timeLabel
    }

    private fun setVolumeBar() {
        val volumeBar = findViewById<SeekBar>(R.id.volumeBar)
        volumeBar.setOnSeekBarChangeListener(
            object : SeekBar.OnSeekBarChangeListener {
                override fun onProgressChanged(
                    seekBar: SeekBar?,
                    progress: Int,
                    fromUser: Boolean
                ) {
                    if (fromUser) {
                        val volumeNum = progress / 100.0f
                        mp.setVolume(volumeNum, volumeNum)
                    }
                }

                override fun onStartTrackingTouch(seekBar: SeekBar?) {
                }

                override fun onStopTrackingTouch(seekBar: SeekBar?) {
                }
            }
        )
    }

    private fun setPositionBar() {
        positionBar.max = mp.duration
        positionBar.setOnSeekBarChangeListener(
            object : SeekBar.OnSeekBarChangeListener {
                override fun onProgressChanged(
                    seekBar: SeekBar?,
                    progress: Int,
                    fromUser: Boolean
                ) {
                    if (fromUser) {
                        mp.seekTo(progress)
                    }
                }

                override fun onStartTrackingTouch(seekBar: SeekBar?) {
                }

                override fun onStopTrackingTouch(seekBar: SeekBar?) {
                }
            }
        )
    }

    private fun setPlayBtn() {
        playBtn.setOnClickListener {
            if (mp.isPlaying) {
                mp.pause()
                it.setBackgroundResource(R.drawable.play)
            } else {
                mp.start()
                it.setBackgroundResource(R.drawable.stop)
            }
        }
    }


    private fun setSongPreview(uri : Uri){
        val mmr = MediaMetadataRetriever()
        mmr.setDataSource(applicationContext, uri)
        val picArr = mmr.embeddedPicture
        if (picArr != null) {
            val picBmp = BitmapFactory.decodeByteArray(picArr, 0, picArr!!.size)
            image.setImageBitmap(Bitmap.
            createScaledBitmap(picBmp, 200, 200, false ))
        }
    }
}