package via.sep4.Deprecated;

import android.os.Bundle;

import androidx.fragment.app.Fragment;

import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;

import via.sep4.R;

@Deprecated
public class DiaryDetails extends Fragment
{
public DiaryDetails()
{
	// Required empty public constructor
}

@Override
public void onCreate(Bundle savedInstanceState)
{
	super.onCreate(savedInstanceState);
}

@Override
public View onCreateView(LayoutInflater inflater, ViewGroup container,
						 Bundle savedInstanceState)
{
	View root = inflater.inflate(R.layout.fragment_diary_details, container, false);
	return root;
}
}