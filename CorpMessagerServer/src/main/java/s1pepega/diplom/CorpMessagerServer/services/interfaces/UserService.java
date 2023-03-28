package s1pepega.diplom.CorpMessagerServer.services.interfaces;

import org.springframework.lang.NonNull;
import s1pepega.diplom.CorpMessagerServer.entities.User;

import java.util.List;

public interface UserService {

    @NonNull
    List<User> findAll();

    @NonNull
    User findById(@NonNull Integer id);

    @NonNull
    User findByName(@NonNull String name);

    @NonNull
    User login(@NonNull User potentialUser);

    @NonNull
    User createUser(@NonNull User newUser);

    @NonNull
    User update(@NonNull User updatedUserInfo);

    void delete(@NonNull Integer id);
}
