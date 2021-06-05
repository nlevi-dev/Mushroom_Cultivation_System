package via.sep4.Deprecated;

import android.os.Bundle;
import android.util.Log;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.TextView;

import androidx.fragment.app.Fragment;

import retrofit2.Call;
import retrofit2.Callback;
import retrofit2.Response;
import via.sep4.Model.User;
import via.sep4.Persistence.UserAPI;
import via.sep4.Persistence.WebClient;
import via.sep4.R;

@Deprecated
public class RetrofitTestActivity extends Fragment {

    // TODO: Rename parameter arguments, choose names that match
    // the fragment initialization parameters, e.g. ARG_ITEM_NUMBER
    private static final String ARG_PARAM1 = "param1";
    private static final String ARG_PARAM2 = "param2";

    // TODO: Rename and change types of parameters
    private String mParam1;
    private String mParam2;

    private TextView textViewUserBob;

    public RetrofitTestActivity() {
        // Required empty public constructor
    }

    /**
     * Use this factory method to create a new instance of
     * this fragment using the provided parameters.
     *
     * @param param1 Parameter 1.
     * @param param2 Parameter 2.
     * @return A new instance of fragment Dashboard.
     */
    // TODO: Rename and change types and number of parameters
    public static RetrofitTestActivity newInstance(String param1, String param2) {
        RetrofitTestActivity fragment = new RetrofitTestActivity();
        Bundle args = new Bundle();
        args.putString(ARG_PARAM1, param1);
        args.putString(ARG_PARAM2, param2);
        fragment.setArguments(args);
        return fragment;
    }

    @Override
    public void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        Log.i("OnCreate", "ONCREATE TRIGGERED = RETROFIT ACTIVITY");
        if (getArguments() != null) {
            mParam1 = getArguments().getString(ARG_PARAM1);
            mParam2 = getArguments().getString(ARG_PARAM2);
        }
        UserAPI userAPI = WebClient.getUserAPI();
        textViewUserBob = getView().findViewById(R.id.GetUserBob);
        Call<User> getUserBob = userAPI.getUserBob();
        getUserBob.enqueue(new Callback<User>() {
            @Override
            public void onResponse(Call<User> call, Response<User> response) {
                //Response checks
                if (response.code() != 200) {
                    textViewUserBob.setText("Check Connection");
                } else {
                    Log.i("RESPONSE", "Response= " + response.body().getUsername());
                    String responseText = "";
                    responseText = response.body().getUsername();
                    textViewUserBob.setText(responseText);
                }
            }

            @Override
            public void onFailure(Call<User> call, Throwable t) {

            }
        });
    }

    @Override
    public View onCreateView(LayoutInflater inflater, ViewGroup container,
                             Bundle savedInstanceState) {
        // Inflate the layout for this fragment
        return inflater.inflate(R.layout.fragment_retrofit_test, container, false);
    }
}
