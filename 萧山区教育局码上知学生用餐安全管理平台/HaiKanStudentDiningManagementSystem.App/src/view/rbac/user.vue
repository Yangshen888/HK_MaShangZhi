<template>
  <div>
    <Card>
      <dz-table
        :totalCount="stores.user.query.totalCount"
        :pageSize="stores.user.query.pageSize"
        @on-page-change="handlePageChanged"
        @on-page-size-change="handlePageSizeChanged"
      >
        <div slot="searcher">
          <section class="dnc-toolbar-wrap">
            <Row :gutter="16">
              <Col span="16">
                <Form inline @submit.native.prevent>
                  <FormItem>
                    <Input
                      type="text"
                      search
                      :clearable="true"
                      v-model="stores.user.query.kw"
                      placeholder="输入关键字搜索..."
                      @on-search="handleSearchUser"
                    >
                      <Select
                        slot="prepend"
                        v-model="stores.user.query.isDeleted"
                        @on-change="handleSearchUser"
                        placeholder="删除状态"
                        style="width:60px;"
                      >
                        <Option
                          v-for="item in stores.user.sources.isDeletedSources"
                          :value="item.value"
                          :key="item.value"
                        >{{item.text}}</Option>
                      </Select>
                      <Select
                        slot="prepend"
                        v-model="stores.user.query.userType"
                        @on-change="handleSearchUser2"
                        placeholder="用户类型"
                        style="width:120px;"
                      >
                        <Option
                          v-for="item in stores.user.sources.userTypes2"
                          :value="item.value"
                          :key="item.value"
                        >{{item.text}}</Option>
                      </Select>
                    </Input>
                  </FormItem>
                  <FormItem>
                    <Input
                      v-if="stores.user.query.userType==1"
                      type="text"
                      search
                      style="width:200px"
                      :clearable="true"
                      v-model="stores.user.query.phone"
                      placeholder="输入手机号搜索..."
                      @on-search="handleSearchUser"
                    />
                  </FormItem>
                </Form>
              </Col>
              <Col span="8" class="dnc-toolbar-btns">
                <ButtonGroup class="mr3">
                  <Button
                    v-if="stores.user.query.userType==0"
                    v-can="'delete'"
                    class="txt-danger"
                    icon="md-trash"
                    title="删除"
                    @click="handleBatchCommand('delete')"
                  ></Button>
                  <Button
                    v-if="stores.user.query.userType==0"
                    v-can="'recover'"
                    class="txt-success"
                    icon="md-redo"
                    title="恢复"
                    @click="handleBatchCommand('recover')"
                  ></Button>
                  <!--                  <Button-->
                  <!--                    class="txt-danger"-->
                  <!--                    icon="md-hand"-->
                  <!--                    title="禁用"-->
                  <!--                    @click="handleBatchCommand('forbidden')"-->
                  <!--                  ></Button>-->
                  <!--                  <Button-->
                  <!--                    class="txt-success"-->
                  <!--                    icon="md-checkmark"-->
                  <!--                    title="启用"-->
                  <!--                    @click="handleBatchCommand('normal')"-->
                  <!--                  ></Button>-->
                  <Button icon="md-refresh" title="刷新" @click="handleRefresh"></Button>
                </ButtonGroup>
                <Button
                  v-if="stores.user.query.userType==0"
                  v-can="'create'"
                  icon="md-create"
                  type="primary"
                  @click="handleShowCreateWindow"
                  title="新增用户"
                >新增用户</Button>
              </Col>
            </Row>
          </section>
        </div>
        <Table
          slot="table"
          ref="tables"
          :border="false"
          size="small"
          :highlight-row="true"
          :data="stores.user.data"
          :columns="stores.user.columns"
          @on-select="handleSelect"
          @on-selection-change="handleSelectionChange"
          @on-refresh="handleRefresh"
          :row-class-name="rowClsRender"
          @on-page-change="handlePageChanged"
          @on-page-size-change="handlePageSizeChanged"
          @on-sort-change="handleSortChange"
        >
          <template slot-scope="{row,index}" slot="userType">
            <span>{{renderUserType(row.userType)}}</span>
          </template>
          <template slot-scope="{row,index}" slot="isUp1">
            <span>{{renderUpStatus(row.isUploadVideo)}}</span>
          </template>
          <template slot-scope="{row,index}" slot="isUp2">
            <span>{{renderUpStatus(row.isUploadPicture)}}</span>
          </template>
          <template slot-scope="{row,index}" slot="isUp3">
            <span>{{renderUpStatus(row.isAddPrecord)}}</span>
          </template>
          <template slot-scope="{row,index}" slot="isUp4">
            <span>{{renderUpStatus(row.isFlow)}}</span>
          </template>
          <template slot-scope="{row,index}" slot="status">
            <Tag :color="renderStatus(row.status).color">{{renderStatus(row.status).text}}</Tag>
          </template>
          <template slot-scope="{ row, index }" slot="action">
            <Poptip
              v-if="(!row._disabled)&&row.type!=5"
              confirm
              :transfer="true"
              title="确定要删除吗?"
              @on-ok="handleDelete(row)"
            >
              <Tooltip placement="top" content="删除" :delay="1000" :transfer="true">
                <Button type="error" size="small" shape="circle" icon="md-trash"></Button>
              </Tooltip>
            </Poptip>
            <Tooltip placement="top" content="编辑" :delay="1000" :transfer="true">
              <Button
                v-if="row.type!=5"
                v-can="'edit'"
                type="primary"
                size="small"
                shape="circle"
                icon="md-create"
                @click="handleEdit(row)"
              ></Button>
            </Tooltip>
            <Tooltip placement="top" content="是否启用视频上传" :delay="1000" :transfer="true">
              <Button
                v-if="row.type==5"
                v-can="'isUp'"
                type="success"
                size="small"
                shape="circle"
                icon="md-contacts"
                @click="handleChangeIsUp(row)"
              ></Button>
            </Tooltip>
          </template>
        </Table>
      </dz-table>
    </Card>
    <Drawer
      :title="formTitle"
      v-model="formModel.opened"
      width="600"
      :mask-closable="false"
      :mask="true"
      :styles="styles"
    >
      <Form :model="formModel.fields" ref="formUser" :rules="formModel.rules" label-position="top">
        <Row :gutter="16">
          <Col span="12">
            <FormItem label="登录名" prop="loginName">
              <Input v-model="formModel.fields.loginName" placeholder="请输入登录名" />
            </FormItem>
          </Col>
          <Col span="12">
            <FormItem label="真实名" prop="realName">
              <Input v-model="formModel.fields.realName" placeholder="请输入真实名" />
            </FormItem>
          </Col>
        </Row>
        <Row :gutter="16">
          <Col span="12">
            <FormItem label="密码" prop="passWord">
              <Input type="password" v-model="formModel.fields.passWord" v-show="false" />
              <Input type="password" v-model="formModel.fields.passWord" placeholder="请输入登录密码" />
            </FormItem>
          </Col>
          <Col span="12">
            <FormItem label="角色" prop="systemRoleUuid">
              <Select filterable v-model="formModel.fields.systemRoleUuid" @on-change="changeRole">
                <Option
                  v-for="item in rolelist"
                  :value="item.systemRoleUuid"
                  :key="item.systemRoleUuid"
                  :disabled="item.disabled"
                >{{item.roleName}}</Option>
              </Select>
            </FormItem>
          </Col>
        </Row>
        <Row :gutter="16">
          <Col span="12">
            <FormItem label="身份证号" prop="userIdCard">
              <Input v-model="formModel.fields.userIdCard" placeholder="请输入身份证号" :maxlength="18" />
            </FormItem>
          </Col>
          <Col v-show="isShowSchool" span="12">
            <FormItem label="学校">
              <Select v-model="formModel.fields.schoolUuid">
                <Option
                  v-for="item in schoolList"
                  :value="item.schoolUuid"
                  :key="item.schoolUuid"
                >{{item.schoolName}}</Option>
              </Select>
            </FormItem>
          </Col>
        </Row>
        <Row :gutter="16" v-if="this.$store.state.user.schoolguid!=null">
          <Col span="12">
            <FormItem label="是否为食堂工作人员">
              <i-switch
                size="large"
                v-model="formModel.fields.isStaff"
                :true-value="1"
                :false-value="0"
                @on-change="changeSwitch"
              >
                <span slot="open">是</span>
                <span slot="close">否</span>
              </i-switch>
            </FormItem>
          </Col>
        </Row>
        <Row :gutter="16" v-show="isShowJog">
          <Col span="12">
            <FormItem label="职务" prop="job">
              <Input v-model="formModel.fields.job" placeholder="请输入职务" :maxlength="18" />
            </FormItem>
          </Col>
          <Col span="12">
            <FormItem label="有无健康证">
              <Select v-model="formModel.fields.healthCertificate">
                <Option
                  v-for="item in stores.user.sources.healthCertificate"
                  :value="item.value"
                  :key="item.value"
                >{{item.text}}</Option>
              </Select>
            </FormItem>
          </Col>
        </Row>
      </Form>
      <div class="demo-drawer-footer">
        <Button icon="md-checkmark-circle" type="primary" @click="handleSubmitUser">保 存</Button>
        <Button style="margin-left: 8px" icon="md-close" @click="formModel.opened = false">取 消</Button>
      </div>
    </Drawer>
    <Drawer
      :title="'上传权限修改'"
      v-model="formUp.opened"
      width="400"
      :mask-closable="false"
      :mask="true"
      :styles="styles"
    >
      <Form :model="formUp.fields" ref="formUp" :rules="formUp.rules" label-position="top">
        <Row :gutter="16">
          <Col span="16">
            <FormItem label="学校" prop="schoolUuid">
              <Select v-model="formUp.fields.schoolUuid">
                <Option
                  v-for="item in schoolList"
                  :value="item.schoolUuid"
                  :key="item.schoolUuid"
                >{{item.schoolName}}</Option>
              </Select>
            </FormItem>
          </Col>
        </Row>
        <Row :gutter="16">
          <Col span="16">
            <FormItem label="是否赋予厨房视频上传权限">
              <i-switch v-model="formUp.fields.isUploadVideo" >
                <span slot="open">是</span>
                <span slot="close">否</span>
              </i-switch> 
            </FormItem>
          </Col>
        </Row>
        <Row :gutter="16">
          <Col span="16">
            <FormItem label="是否赋予每餐实拍上传权限">
              <i-switch v-model="formUp.fields.isUploadPicture" >
                <span slot="open">是</span>
                <span slot="close">否</span>
              </i-switch> 
            </FormItem>
          </Col>
        </Row>
        <Row :gutter="16">
          <Col span="16">
            <FormItem label="是否赋予采购记录上传权限">
              <i-switch v-model="formUp.fields.isAddPrecord" >
                <span slot="open">是</span>
                <span slot="close">否</span>
              </i-switch> 
            </FormItem>
          </Col>
        </Row>
        <Row :gutter="16">
          <Col span="16">
            <FormItem label="是否赋予成菜流程上传权限">
              <i-switch v-model="formUp.fields.isFlow" >
                <span slot="open">是</span>
                <span slot="close">否</span>
              </i-switch> 
            </FormItem>
          </Col>
        </Row>
      </Form>
      <div class="demo-drawer-footer">
        <Button icon="md-checkmark-circle" type="primary" @click="handleSubmitUP">保 存</Button>
        <Button style="margin-left: 8px" icon="md-close" @click="formUp.opened = false">取 消</Button>
      </div>
    </Drawer>
  </div>
</template>

<script>
import DzTable from "_c/tables/dz-table.vue";
import {
  getUserList,
  createUser,
  loadUser,
  editUser,
  deleteUser,
  batchCommand,
  loadUser2,
  editUp,
} from "@/api/rbac/user";
import { loadRoleListByUserGuid, loadSimpleList } from "@/api/rbac/role";
import { getSchoolList2 } from "@/api/rbac/school";
import { loaddepartmentListDetail } from "@/api/rbac/department";
import {
  globalvalidateIDCardNoMust,
  globalvalidatepwd,
  globalvalidateIsNotEmpty,
} from "@/global/validate";
export default {
  name: "rbac_user_page",
  components: {
    DzTable,
  },
  data() {
    return {
      commands: {
        delete: { name: "delete", title: "删除" },
        recover: { name: "recover", title: "恢复" },
        // forbidden: { name: "forbidden", title: "禁用" },
        // normal: { name: "normal", title: "启用" }
      },
      isUP:false,
      isShowSchool: false,
      isShowJog: false,
      formModel: {
        opened: false,
        title: "创建用户",
        mode: "create",
        selection: [],
        fields: {
          loginName: "",
          realName: "",
          passWord: "",
          addPeople: "",
          schoolUuid: "",
          //avatar: "",
          userType: 0,
          //isLocked: 0,
          //status: 1,
          isDeleted: 0,
          systemRoleUuid: "",
          userIdCard: "",
          departmentUuid: "",
          job: "",
          healthCertificate: "",
          isStaff: 0,
          isUpload: false,
          phone: "",
        },
        rules: {
          loginName: [
            {
              validator: globalvalidateIsNotEmpty,
              type: "string",
              required: true,
              message: "请输入登录名",
              min: 3,
            },
          ],
          realName: [
            {
              validator: globalvalidateIsNotEmpty,
              type: "string",
              required: true,
              message: "请输入真实名",
            },
          ],
          passWord: [
            { validator: globalvalidatepwd, type: "string", required: true },
          ],
          systemRoleUuid: [{ required: true, message: "请选择角色" }],
          userIdCard: [
            {
              validator: globalvalidateIDCardNoMust,
              message: "请输入正确的身份证号",
              trigger: "blur",
            },
          ],
        },
      },
      formUp: {
        userGuid: "",
        opened: false,
        ownedRoles: [],
        inited: false,
        fields: {
          loginName: "",
          realName: "",
          passWord: "",
          addPeople: "",
          schoolUuid: "",
          //avatar: "",
          userType: 0,
          //isLocked: 0,
          //status: 1,
          isDeleted: 0,
          systemRoleUuid: "",
          userIdCard: "",
          departmentUuid: "",
          job: "",
          healthCertificate: "",
          isStaff: 0,
          isUploadVideo: false,
          isUploadPicture:false,
          isAddPrecord:false,
          isFlow:false,
          phone: "",
        },
        rules: {
          schoolUuid: [{ required: true, message: "请选择角色" }],
        },
      },
      stores: {
        user: {
          query: {
            totalCount: 0,
            pageSize: 20,
            currentPage: 1,
            kw: "",
            userType: 0,
            phone: "",
            isDeleted: 0,
            status: -1,
            sort: [
              {
                direct: "DESC",
                field: "ID",
              },
            ],
          },
          sources: {
            userTypes: [
              { value: 0, text: "超级管理员" },
              { value: 1, text: "管理员" },
              { value: 2, text: "普通用户" },
            ],
            userTypes2: [
              { value: 0, text: "后台用户" },
              { value: 1, text: "微信用户" },
            ],
            isDeletedSources: [
              { value: -1, text: "全部" },
              { value: 0, text: "正常" },
              { value: 1, text: "已删" },
            ],
            statusSources: [
              { value: -1, text: "全部" },
              { value: 0, text: "禁用" },
              { value: 1, text: "正常" },
            ],
            statusFormSources: [
              { value: 0, text: "禁用" },
              { value: 1, text: "正常" },
            ],
            healthCertificate: [
              { value: 0, text: "无" },
              { value: 1, text: "有" },
            ],
          },
          columns: [
            { type: "selection", width: 50, key: "handle" },
            { title: "登录名", key: "loginName", sortable: true },
            { title: "真实名", key: "realName" },
            { title: "身份证号", key: "userIdCard" },
            // { title: "科室名", key: "departmentName"},
            { title: "用户类型", key: "userType", slot: "userType" },

            {
              title: "创建时间",
              ellipsis: true,
              tooltip: true,
              key: "addTime",
            },
            { title: "创建者", key: "addPeople" },
            {
              title: "操作",
              align: "center",
              width: 150,
              className: "table-command-column",
              slot: "action",
            },
          ],
          columns1: [
            { type: "selection", width: 50, key: "handle" },
            { title: "登录名", key: "loginName", sortable: true },
            { title: "真实名", key: "realName" },
            { title: "身份证号", key: "userIdCard" },
            // { title: "科室名", key: "departmentName"},
            { title: "用户类型", key: "userType", slot: "userType" },

            {
              title: "创建时间",
              ellipsis: true,
              tooltip: true,
              key: "addTime",
            },
            { title: "创建者", key: "addPeople" },
            {
              title: "操作",
              align: "center",
              width: 150,
              className: "table-command-column",
              slot: "action",
            },
          ],
          columns2: [
            { type: "selection", width: 50, key: "handle" },
            { title: "登录名", key: "loginName", sortable: true },
            // { title: "真实名", key: "realName" },
            { title: "用户类型", key: "userType", slot: "userType" },
            {
              title: "创建时间",
              ellipsis: true,
              tooltip: true,
              key: "addTime",
            },
            { title: "电话", key: "phone" },
            { title: "学校", key: "schoolName" },
            { title: "上传厨房视频权限", key: "isUploadVideo", width: 120, slot: "isUp1" },
            { title: "上传每餐实拍权限", key: "isUploadPicture", width: 120, slot: "isUp2" },
            { title: "上传采购记录权限", key: "isAddPrecord", width: 120, slot: "isUp3" },
            { title: "上传采购流程权限", key: "isFlow", width: 120, slot: "isUp4" },
            {
              title: "操作",
              align: "center",
              width: 150,
              className: "table-command-column",
              slot: "action",
            },
          ],
          data: [],
        },
      },
      styles: {
        height: "calc(100% - 55px)",
        overflow: "auto",
        paddingBottom: "53px",
        position: "static",
      },
      rolelist: [],
      departmentlist: [],
      schoolList: [],
      switch: false,
    };
  },
  computed: {
    formTitle() {
      if (this.formModel.mode === "create") {
        return "创建用户";
      }
      if (this.formModel.mode === "edit") {
        return "编辑用户";
      }
      return "";
    },
    selectedRows() {
      return this.formModel.selection;
    },
    selectedRowsId() {
      return this.formModel.selection.map((x) => x.systemUserUuid);
    },
  },
  methods: {
    loadUserList() {
      getUserList(this.stores.user.query).then((res) => {
        this.stores.user.data = res.data.data;
        this.stores.user.query.totalCount = res.data.totalCount;
      });
    },
    doloadRoleList() {
      loadSimpleList().then((res) => {
        this.rolelist = res.data.data;
      });
    },
    // doloadDepartmentListdetail(){
    //     loaddepartmentListDetail().then(res=>{
    //         console.log(res.data)
    //         this.departmentlist = res.data.data;
    //     })
    // },
    handleOpenFormWindow() {
      this.formModel.opened = true;
    },
    handleCloseFormWindow() {
      this.formModel.opened = false;
    },
    handleSwitchFormModeToCreate() {
      this.formModel.mode = "create";
    },
    handleSwitchFormModeToEdit() {
      this.formModel.mode = "edit";
      this.handleOpenFormWindow();
    },
    handleEdit(row) {
      this.loadschoollist();
      this.formModel.fields.isStaff = 0;
      this.isShowJog = false;
      this.handleSwitchFormModeToEdit();
      this.handleResetFormUser();
      this.doLoadUser(row.systemUserUuid);
      this.doloadRoleList();
    },
    handleChangeIsUp(row) {
      this.loadschoollist();
      console.log(this.$store.state.user);
      // if(this.$store.state.user.schoolguid!=null){
      //   this.formUp.fields.schoolUuid==this.$store.state.user.schoolguid;
      // }
      this.formUp.opened=true;
      this.doLoadUser2(row.systemUserUuid);
    },
    handleSelect(selection, row) {},
    handleSelectionChange(selection) {
      this.formModel.selection = selection;
    },
    handleRefresh() {
      this.loadUserList();
    },
    handleShowCreateWindow() {
      this.formModel.fields.isStaff = 0;
      this.isShowJog = false;
      this.handleSwitchFormModeToCreate();
      this.handleOpenFormWindow();
      this.handleResetFormUser();
      this.loadschoollist();
      this.doloadRoleList();
    },
    handleSubmitUP(){
      console.log(this.formUp.fields);
      let valid=this.validateUpForm();
      if (valid) {
        editUp(this.formUp.fields).then(res=>{
          if (res.data.code === 200) {
          this.$Message.success(res.data.message);
          this.formUp.opened=false;
          this.loadUserList();
        } else {
          this.$Message.warning(res.data.message);
        }
        });

      }
    },
    handleSubmitUser() {
      let valid = this.validateUserForm();
      //console.log(valid);
      //console.log(this.formModel.fields);
      if (valid) {
        if (this.formModel.mode === "create") {
          this.doCreateUser();
        }
        if (this.formModel.mode === "edit") {
          this.doEditUser();
        }
      }
    },
    handleResetFormUser() {
      this.$refs["formUser"].resetFields();
      this.formModel.fields.job = "";
      this.formModel.fields.healthCertificate = "";
      this.formModel.fields.schoolUuid = "";
    },
    doCreateUser() {
      this.formModel.fields.addPeople = this.$store.state.user.userName;
      createUser(this.formModel.fields).then((res) => {
        if (res.data.code === 200) {
          this.$Message.success(res.data.message);
          this.handleCloseFormWindow();
          this.loadUserList();
        } else {
          this.$Message.warning(res.data.message);
        }
      });
    },
    doEditUser() {
      this.formModel.fields.addPeople = this.$store.state.user.userName;
      editUser(this.formModel.fields).then((res) => {
        if (res.data.code === 200) {
          this.$Message.success(res.data.message);
          this.handleCloseFormWindow();
          this.loadUserList();
        } else {
          this.$Message.warning(res.data.message);
        }
      });
    },
    validateUserForm() {
      let _valid = false;
      this.$refs["formUser"].validate((valid) => {
        if (!valid) {
          this.$Message.error("请完善表单信息");
        } else {
          _valid = true;
        }
      });
      return _valid;
    },
    validateUpForm() {
      let _valid = false;
      this.$refs["formUp"].validate((valid) => {
        if (!valid) {
          this.$Message.error("请完善表单信息");
        } else {
          _valid = true;
        }
      });
      return _valid;
    },
    doLoadUser(systemUserUuid) {
      loadUser({ guid: systemUserUuid }).then((res) => {
        // console.log(res.data.data);
        this.formModel.fields = res.data.data.query;
        this.rolelist = res.data.data.roles;
        console.log(this.formModel.fields);
        let data = this.rolelist.find(
          (x) => x.systemRoleUuid == this.formModel.fields.systemRoleUuid
        );
        if (this.$store.state.user.schoolguid == null) {
          this.isShowSchool = true;
        } else {
          this.isShowSchool = false;
        }
        if (this.formModel.fields.isStaff == 0) {
          this.isShowJog = false;
        } else {
          this.isShowJog = true;
        }
        if (data.roleName == "超级管理员") {
          this.isShowSchool = false;
        }
      });
    },
    doLoadUser2(systemUserUuid){
      loadUser2({ guid: systemUserUuid }).then((res) => {
        // console.log(res.data.data);
        this.formUp.fields = res.data.data;
        console.log(this.formUp.fields);
      });
    },
    handleDelete(row) {
      this.doDelete(row.systemUserUuid);
    },
    doDelete(ids) {
      if (!ids) {
        this.$Message.warning("请选择至少一条数据");
        return;
      }
      deleteUser(ids).then((res) => {
        if (res.data.code === 200) {
          this.$Message.success(res.data.message);
          this.loadUserList();
          this.formModel.selection = [];
        } else {
          this.$Message.warning(res.data.message);
        }
      });
    },
    handleBatchCommand(command) {
      if (!this.selectedRowsId || this.selectedRowsId.length <= 0) {
        this.$Message.warning("请选择至少一条数据");
        return;
      }
      this.$Modal.confirm({
        title: "操作提示",
        content:
          "<p>确定要执行当前 [" +
          this.commands[command].title +
          "] 操作吗?</p>",
        loading: true,
        onOk: () => {
          this.doBatchCommand(command);
        },
      });
    },
    doBatchCommand(command) {
      batchCommand({
        command: command,
        ids: this.selectedRowsId.join(","),
      }).then((res) => {
        if (res.data.code === 200) {
          this.$Message.success(res.data.message);
          this.loadUserList();
          this.formModel.selection = [];
        } else {
          this.$Message.warning(res.data.message);
        }
        this.$Modal.remove();
      });
    },
    handleSearchUser() {
      this.loadUserList();
    },
    handleSearchUser2() {
      this.stores.user.query.phone = "";
      this.formModel.selection = [];
      if (this.stores.user.query.userType == 0) {
        this.stores.user.columns = this.stores.user.columns1;
      } else {
        this.stores.user.columns = this.stores.user.columns2;
      }
      this.loadUserList();
    },
    rowClsRender(row, index) {
      if (row.isDeleted) {
        return "table-row-disabled";
      }
      return "";
    },
    handleSortChange(column) {
      this.stores.user.query.sort.direction = column.order;
      this.stores.user.query.sort.field = column.key;
      this.loadUserList();
    },
    handlePageChanged(page) {
      this.stores.user.query.currentPage = page;
      this.loadUserList();
    },
    handlePageSizeChanged(pageSize) {
      this.stores.user.query.pageSize = pageSize;
      this.loadUserList();
    },
    renderUserType(userType) {
      // console.log(userType);
      var userTypeText = "未知";
      if (userType != null && userType != "") {
        userTypeText = userType;
      }
      // switch (userType) {
      //   case 0:
      //     userTypeText = "超级管理员";
      //     break;
      //   case 1:
      //     userTypeText = "管理员";
      //     break;
      //   case 2:
      //     userTypeText = "普通用户";
      //     break;
      // }
      return userTypeText;
    },
    renderStatus(status) {
      let _status = {
        color: "success",
        text: "正常",
      };
      switch (status) {
        case 0:
          _status.text = "禁用";
          _status.color = "error";
          break;
      }
      return _status;
    },
    changeRole(e) {
      console.log(e);
      if (e == null || e == undefined) {
        return;
      }
      let data = this.rolelist.find((x) => x.systemRoleUuid == e);
      if (this.$store.state.user.schoolguid != null) {
        this.isShowSchool = false;
      } else {
        if (data.roleName == "教育局" || data.roleName == "超级管理员") {
          this.isShowSchool = false;
        } else {
          this.isShowSchool = true;
        }
      }
    },
    changeSwitch(e) {
      console.log(e);
      if (e) {
        this.formModel.fields.job = "";
        this.formModel.fields.healthCertificate = "";
        this.isShowJog = true;
      } else {
        this.isShowJog = false;
        this.formModel.fields.job = "";
        this.formModel.fields.healthCertificate = "";
      }
    },
    loadschoollist() {
      getSchoolList2().then((res) => {
        console.log(res);
        this.schoolList = res.data.data;
        this.schoolList.push({
          schoolUuid: "",
          schoolName: "教育局",
          disabled: false,
        });
        if (
          this.$store.state.user.schoolguid != null &&
          this.$store.state.user.schoolguid != ""
        ) {
          this.schoolList = this.schoolList.filter(
            (x) => x.schoolUuid == this.$store.state.user.schoolguid
          );
          this.formModel.fields.schoolUuid = this.$store.state.user.schoolguid;
        }
      });
    },
    renderUpStatus(state) {
      console.log(state);
      if (state) {
        return "有";
      } else {
        return "无";
      }
    },
  },
  mounted() {
    console.log(this.$store.state.user);
    this.loadUserList();
    this.doloadRoleList();

    getSchoolList2().then((res) => {
      console.log(res);
      this.schoolList = res.data.data;
      this.schoolList.push({
        schoolUuid: "",
        schoolName: "教育局",
        disabled: false,
      });
      if (
        this.$store.state.user.schoolguid != null &&
        this.$store.state.user.schoolguid != ""
      ) {
        this.schoolList = this.schoolList.filter(
          (x) => x.schoolUuid == this.$store.state.user.schoolguid
        );
        this.formModel.fields.schoolUuid = this.$store.state.user.schoolguid;
      }
    });
    //this.doloadDepartmentListdetail();
  },
};
</script>

<style>
</style>
