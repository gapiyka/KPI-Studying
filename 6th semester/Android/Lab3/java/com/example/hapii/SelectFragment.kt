package com.example.hapii

import android.content.Context
import android.os.Bundle
import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import android.widget.*
import androidx.fragment.app.Fragment

class SelectFragment : Fragment() {

    interface OnSelectedListener {
        fun onSelected(text: String, font: Int)
    }

    private lateinit var onSelectedListener: OnSelectedListener

    override fun onAttach(context: Context) {
        super.onAttach(context)
        onSelectedListener = context as OnSelectedListener
    }

    override fun onCreateView(
        inflater: LayoutInflater, container: ViewGroup?,
        savedInstanceState: Bundle?
    ): View? {
        val view: View = inflater.inflate(R.layout.fragment_select, container, false)

        val textInput = view.findViewById<TextView>(R.id.etInput)
        val buttonOk = view.findViewById<Button>(R.id.btnOk)
        val buttonCancel = view.findViewById<Button>(R.id.btnCancel)
        val radioGroup = view.findViewById<RadioGroup>(R.id.radioGroup)
        val empty = getString(R.string.empty)

        buttonOk.setOnClickListener {
            val radioButtonID: Int = radioGroup.checkedRadioButtonId
            val radioButton: View = radioGroup.findViewById(radioButtonID)
            val idx: Int = radioGroup.indexOfChild(radioButton)
            val text = textInput.text.toString()

            if (text.isEmpty())
                Toast.makeText(activity, getString(R.string.empty_input),
                    Toast.LENGTH_SHORT,).show()
            else
                onSelectedListener.onSelected(text, idx)
        }

        buttonCancel.setOnClickListener {
            textInput.text = empty
            onSelectedListener.onSelected(empty, 0)
        }

        return view
    }
}