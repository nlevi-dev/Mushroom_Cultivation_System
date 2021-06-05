package via.sep4.Viewspecimen;

import android.os.Bundle;

import androidx.fragment.app.Fragment;
import androidx.navigation.NavController;
import androidx.navigation.Navigation;

import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.Button;
import android.widget.RadioButton;
import android.widget.RadioGroup;

import java.util.List;

import retrofit2.Call;
import retrofit2.Callback;
import retrofit2.Response;
import via.sep4.Model.Mushroom;
import via.sep4.Model.Status;
import via.sep4.Persistence.WebClient;
import via.sep4.R;

public class selectStage extends Fragment
{
	
	Button saveButton;
	Status status = null;
	int id = -1;
	
	public selectStage()
	{
		// Required empty public constructor
	}
	
	@Override
	public void onCreate(Bundle savedInstanceState)
	{
		super.onCreate(savedInstanceState);
	}
	
	@Override
	public View onCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
	{
		// Inflate the layout for this fragment
		View root = inflater.inflate(R.layout.fragment_select_stage, container, false);
		saveButton = root.findViewById(R.id.saveStage);
		id = getArguments().getInt("id");
		status = (Status) getArguments().getSerializable("status");
		saveButton.setOnClickListener(new View.OnClickListener()
		{
			@Override
			public void onClick(View v)
			{
				/*Call<Status> call = WebClient.getStatusAPI().updateStatus(status,id);
				call.enqueue(new Callback<Status>()
				{
					
					@Override
					public void onResponse(Call<Status> call, Response<Status> response)
					{
					
					}
					
					@Override
					public void onFailure(Call<Status> call, Throwable t)
					{
					
					}
				});*/
				Call<Status> call = WebClient.getStatusAPI().createStatus(status.getSpecimen_key(), status);
				call.enqueue(new Callback<Status>() {
					@Override
					public void onResponse(Call<Status> call, Response<Status> response) {

					}

					@Override
					public void onFailure(Call<Status> call, Throwable t) {

					}
				});
				NavController nav = Navigation.findNavController(root);
				nav.navigate(R.id.action_selectStage_to_dashboard);
			}
		});
		RadioGroup rb = (RadioGroup) root.findViewById(R.id.radioGroup);
		rb.setOnCheckedChangeListener(new RadioGroup.OnCheckedChangeListener() {
			public void onCheckedChanged(RadioGroup group, int checkedId) {
				switch(checkedId) {
					case R.id.radio1:
							status.setStage_name("Inoculation");
						break;
					case R.id.radio2:
							status.setStage_name("Casing");
						break;
					case R.id.radio3:
							status.setStage_name("Spawn Run");
						break;
					case R.id.radio4:
							status.setStage_name("Pinning");
						break;
					case R.id.radio5:
							status.setStage_name("Fruiting");
						break;
					case R.id.radio6:
							status.setStage_name("Dead");
						break;
				}
			}
			
		});
		return root;
	}
	
}