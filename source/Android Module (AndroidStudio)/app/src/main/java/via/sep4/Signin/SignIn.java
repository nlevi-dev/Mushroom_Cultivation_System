package via.sep4.Signin;

import android.os.Bundle;

import androidx.annotation.NonNull;
import androidx.annotation.Nullable;
import androidx.fragment.app.Fragment;
import androidx.lifecycle.Observer;
import androidx.navigation.NavController;
import androidx.navigation.Navigation;

import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.Button;
import android.widget.EditText;

import com.google.android.material.bottomnavigation.BottomNavigationView;

import via.sep4.Model.User;
import via.sep4.Persistence.LocalPersistence;
import via.sep4.R;

/**
 * A simple {@link Fragment} subclass.
 * Use the {@link SignIn#newInstance} factory method to
 * create an instance of this fragment.
 */
public class SignIn extends Fragment {

    // TODO: Rename parameter arguments, choose names that match
    // the fragment initialization parameters, e.g. ARG_ITEM_NUMBER
    private static final String ARG_PARAM1 = "param1";
    private static final String ARG_PARAM2 = "param2";

    // TODO: Rename and change types of parameters
    private String mParam1;
    private String mParam2;

    private SignInViewModel signInViewModel;

    public SignIn() {
        // Required empty public constructor
    }

    /**
     * Use this factory method to create a new instance of
     * this fragment using the provided parameters.
     *
     * @param param1 Parameter 1.
     * @param param2 Parameter 2.
     * @return A new instance of fragment SignIn.
     */
    // TODO: Rename and change types and number of parameters
    public static SignIn newInstance(String param1, String param2) {
        SignIn fragment = new SignIn();
        Bundle args = new Bundle();
        args.putString(ARG_PARAM1, param1);
        args.putString(ARG_PARAM2, param2);
        fragment.setArguments(args);
        return fragment;
    }

    @Override
    public void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        signInViewModel = new SignInViewModel();
        final Observer<User> userObserver = new Observer<User>()
        {
            @Override
            public void onChanged(@Nullable final User user)
            {

            }
        };
        signInViewModel.getUser().observe(this, userObserver);
        if (getArguments() != null) {
            mParam1 = getArguments().getString(ARG_PARAM1);
            mParam2 = getArguments().getString(ARG_PARAM2);
        }
    }

    @Override
    public View onCreateView(LayoutInflater inflater, ViewGroup container,
                             Bundle savedInstanceState) {
        // Inflate the layout for this fragment
        View v =  inflater.inflate(R.layout.fragment_sign_in, container, false);
        return v;
    }

    @Override
    public void onViewCreated(@NonNull View view, @Nullable Bundle savedInstanceState) {
        super.onViewCreated(view, savedInstanceState);
        Button button = view.findViewById(R.id.buttonSignIn);
        button.setOnClickListener(v -> {
            signInViewModel.getUser().getValue().setUsername(((EditText) view.findViewById(R.id.editTextTextPersonName)).getText().toString());
            signInViewModel.getUser().getValue().setPassword(((EditText) view.findViewById(R.id.editTextTextPersonName2)).getText().toString());
            signInViewModel.SignIn();
            final Observer<Boolean> observer = aBoolean -> {
                if(aBoolean) {
                    System.out.println("Observer");
                    LocalPersistence.getDatabaseInstance(getActivity().getApplicationContext());
                    BottomNavigationView bottomNavigationView = view.getRootView().findViewById(R.id.bottomNavigationView);
                    bottomNavigationView.setVisibility(View.VISIBLE); //Turns on Navigation view
                    NavController nav = Navigation.findNavController(view);
                   // if(nav.getCurrentDestination().getId() == R.id)
                    try
                    {
                        nav.navigate(R.id.action_signIn_to_dashboard);
                    }
                    catch (IllegalArgumentException e)
                    {
                        e.printStackTrace();
                    }
                }
            };
            signInViewModel.getSuccess().observe(getViewLifecycleOwner(), observer);
        });
    }
}