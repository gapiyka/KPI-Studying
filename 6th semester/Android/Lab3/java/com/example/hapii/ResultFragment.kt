package com.example.hapii

import android.content.Context
import android.graphics.Typeface
import android.os.Bundle
import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import android.widget.TextView
import androidx.core.content.res.ResourcesCompat
import androidx.fragment.app.Fragment

class ResultFragment : Fragment() {
    private lateinit var fontsFaces : List<Typeface>
    override fun onCreateView(
        inflater: LayoutInflater, container: ViewGroup?,
        savedInstanceState: Bundle?
    ): View? {
        val activity = activity as Context
        val customTypeface = ResourcesCompat.getFont(activity, R.font.nature_beauty)
        fontsFaces = listOf(Typeface.SANS_SERIF, Typeface.MONOSPACE, Typeface.SERIF, customTypeface!!)
        return inflater.inflate(R.layout.fragment_result, container, false)
    }

    fun setTextResult(text: String, font: Int) {
        val textOutput = requireView().findViewById<TextView>(R.id.tOutput)
        textOutput.text = text
        textOutput.typeface = fontsFaces[font]
    }

}