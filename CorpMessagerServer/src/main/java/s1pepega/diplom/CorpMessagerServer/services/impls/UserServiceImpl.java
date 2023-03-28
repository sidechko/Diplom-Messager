package s1pepega.diplom.CorpMessagerServer.services.impls;

import jakarta.persistence.EntityNotFoundException;
import lombok.RequiredArgsConstructor;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;
import org.springframework.transaction.annotation.Transactional;
import s1pepega.diplom.CorpMessagerServer.entities.User;
import s1pepega.diplom.CorpMessagerServer.repositories.UserRepository;
import s1pepega.diplom.CorpMessagerServer.services.interfaces.UserService;

import java.util.ArrayList;
import java.util.Date;
import java.util.List;
import java.util.Optional;

import static java.util.Optional.ofNullable;

@Service("UserServiceImpl")
@RequiredArgsConstructor
public class UserServiceImpl implements UserService {

    @Autowired
    private UserRepository userRepository;

    @Override
    @Transactional(readOnly = true)
    public List<User> findAll() {
        return new ArrayList<>(userRepository.findAll());
    }

    @Override
    @Transactional(readOnly = true)
    public User findById(Integer id) {
        return userRepository.findById(id)
                .orElseThrow(()->new EntityNotFoundException("User with id "+id+" not found"));
    }

    @Override
    @Transactional(readOnly = true)
    public User findByName(String name) {
        return userRepository.findByName(name)
                .orElseThrow(()->new EntityNotFoundException("User with login "+name+" not found"));
    }

    @Override
    @Transactional(readOnly = true)
    public User login(User userRequest) {
        User user = userRepository.findByName(userRequest.getLogin())
                .orElseThrow(()->new EntityNotFoundException("User with login "+userRequest.getLogin()+" not found"));
        if(!user.getPassword().equals(userRequest.getPassword()))
            throw new EntityNotFoundException("Illegal password. Try again");
        return user;
    }

    @Override
    @Transactional
    public User createUser(User newUser) {
        Optional<User> optionalUser = userRepository.findByName(newUser.getLogin());
        if(optionalUser.isPresent())
            throw new EntityNotFoundException("User with login "+newUser.getLogin()+" was registered.");
        newUser.setRegTime(new Date());
        return userRepository.save(newUser);
    }

    @Override
    @Transactional
    public User update(User updatedUserInfo) {
        User user = userRepository.findById(updatedUserInfo.getId())
                .orElseThrow(()->new EntityNotFoundException("User with id "+updatedUserInfo.getId()+" not found"));
        ofNullable(updatedUserInfo.getLogin()).map(user::setLogin);
        ofNullable(updatedUserInfo.getPassword()).map(user::setPassword);
        userRepository.save(user);
        return user;
    }

    @Override
    @Transactional
    public void delete(Integer id) {
        throw new EntityNotFoundException("You cannot delete message.");
    }
}
