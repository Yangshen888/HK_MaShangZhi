<template>
  <div>
    <Card>
      <dz-table
        :totalCount="stores.ingredient.query.totalCount"
        :pageSize="stores.ingredient.query.pageSize"
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
                      v-model="stores.ingredient.query.kw"
                      placeholder="输入食材名称搜索..."
                      @on-search="handleSearchIngredient()"
                    >
                      <Select
                        slot="prepend"
                        v-model="stores.ingredient.query.kw2"
                        @on-change="handleSearchIngredient"
                        placeholder="类型"
                        style="width:60px;"
                        clearable
                      >
                        <Option
                          v-for="item in stores.ingredient.sources.types"
                          :value="item.typeUuid"
                          :key="item.typeUuid"
                        >{{item.name}}</Option>
                      </Select>
                      <!--                      <Select-->
                      <!--                        slot="prepend"-->
                      <!--                        v-model="stores.user.query.status"-->
                      <!--                        @on-change="handleSearchUser"-->
                      <!--                        placeholder="用户状态"-->
                      <!--                        style="width:60px;"-->
                      <!--                      >-->
                      <!--                        <Option-->
                      <!--                          v-for="item in stores.user.sources.statusSources"-->
                      <!--                          :value="item.value"-->
                      <!--                          :key="item.value"-->
                      <!--                        >{{item.text}}</Option>-->
                      <!--                      </Select>-->
                    </Input>
                  </FormItem>
                </Form>
              </Col>
              <Col span="8" class="dnc-toolbar-btns">
                <ButtonGroup class="mr3">
                  <Button
                    v-can="'delete'"
                    class="txt-danger"
                    icon="md-trash"
                    title="删除"
                    @click="handleBatchCommand('delete')"
                  ></Button>
                  <!-- <Button
                    class="txt-success"
                    icon="md-redo"
                    title="恢复"
                    @click="handleBatchCommand('recover')"
                  ></Button>-->
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
                  v-can="'create'"
                  icon="md-create"
                  type="primary"
                  @click="handleShowCreateWindow"
                  title="新增食材"
                >新增食材</Button>
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
          :data="stores.ingredient.data"
          :columns="stores.ingredient.columns"
          @on-select="handleSelect"
          @on-selection-change="handleSelectionChange"
          @on-refresh="handleRefresh"
          :row-class-name="rowClsRender"
          @on-page-change="handlePageChanged"
          @on-page-size-change="handlePageSizeChanged"
          @on-sort-change="handleSortChange"
        >
          <!-- <template slot-scope="{row,index}" slot="userType">
            <span>{{renderUserType(row.userType)}}</span>
          </template>
          <template slot-scope="{row,index}" slot="status">
            <Tag :color="renderStatus(row.status).color">{{renderStatus(row.status).text}}</Tag>
          </template>-->
          <template slot-scope="{ row, index }" slot="action">
            <Poptip confirm :transfer="true" title="确定要删除吗?" @on-ok="handleDelete(row)">
              <Tooltip placement="top" content="删除" :delay="1000" :transfer="true">
                <Button v-can="'delete'" type="error" size="small" shape="circle" icon="md-trash"></Button>
              </Tooltip>
            </Poptip>
            <Tooltip placement="top" content="编辑" :delay="1000" :transfer="true">
              <Button
                v-can="'edit'"
                type="primary"
                size="small"
                shape="circle"
                icon="md-create"
                @click="handleEdit(row)"
              ></Button>
            </Tooltip>
            <Tooltip placement="top" content="编辑" :delay="1000" :transfer="true">
              <Button
                v-can="'show'"
                type="warning"
                size="small"
                shape="circle"
                icon="md-search"
                @click="handleShow(row)"
              ></Button>
            </Tooltip>
            <!--            <Tooltip placement="top" content="分配角色" :delay="1000" :transfer="true">-->
            <!--              <Button type="success" size="small" shape="circle" icon="md-contacts" @click="handleAssignRole(row)"></Button>-->
            <!--            </Tooltip>-->
          </template>
        </Table>
      </dz-table>
    </Card>
    <Drawer
      :title="formTitle"
      v-model="formModel.opened"
      width="400"
      :mask-closable="false"
      :mask="true"
      :styles="styles"
    >
      <Form
        :model="formModel.fields"
        ref="formIngredient"
        :rules="formModel.rules"
        label-position="top"
      >
        <Row :gutter="16">
          <Col span="14">
            <FormItem label="食材名称" prop="foodName">
              <Input :readonly="checkRead()" style="width:350px" v-model="formModel.fields.foodName" placeholder="请输入食材名称" />
            </FormItem>
          </Col>
        </Row>
        <Row :gutter="16">
          <Col span="14">
            <FormItem label="食材类型" prop="typeUuid">
               <!-- <Input :readonly="checkRead()" style="width:350px" v-model="formModel.fields.typeUuid" placeholder="请输入类型" /> -->
              <Select v-model="formModel.fields.typeUuid" :disabled="checkRead()"  style="width:350px">
                <Option
                  
                  v-for="item in stores.ingredient.sources.types2"
                  :value="item.typeUuid"
                  :key="item.typeUuid"
                >{{item.name}}</Option>
              </Select>
            </FormItem>
          </Col>
        </Row>
        <Row  :gutter="16">
          <Col span="14">
            <FormItem label="热能(Kcal/100g)" prop="heatEnergy">
              <InputNumber :readonly="checkRead()" style="width:350px"  class="inputnum"  v-model="formModel.fields.heatEnergy" :min="0"  placeholder="请输入食材热量" />
            </FormItem>
          </Col>
        </Row>
        <Row  :gutter="16">
          <Col span="14">
            <FormItem label="蛋白质(g/100g)" prop="protein">
              <InputNumber :readonly="checkRead()" class="inputnum" style="width:350px"   v-model="formModel.fields.protein"  :min="0" placeholder="请输入食材蛋白质含量" />
            </FormItem>
          </Col>
        </Row>
        <Row  :gutter="16">
          <Col span="14">
            <FormItem label="脂肪(g/100g)" prop="fat">
              <InputNumber :readonly="checkRead()" class="inputnum" style="width:350px"   v-model="formModel.fields.fat"  :min="0" placeholder="请输入食材名称" />
            </FormItem>
          </Col>
        </Row>
        <Row  :gutter="16">
          <Col span="14">
            <FormItem label="糖类(g/100g)" prop="saccharides">
              <InputNumber :readonly="checkRead()" class="inputnum" style="width:350px"   v-model="formModel.fields.saccharides" :min="0" placeholder="请输入食材名称" />
            </FormItem>
          </Col>
        </Row>
        <Row  :gutter="16">
          <Col span="14">
            <FormItem label="VA(ug/100g)" prop="va">
              <InputNumber :readonly="checkRead()" class="inputnum" style="width:350px"   v-model="formModel.fields.va"  :min="0" placeholder="请输入食材名称" />
            </FormItem>
          </Col>
        </Row>
        <Row v-show="!checkShow()">
          <div class="demo-upload-list" v-for="item in uploadList">
            <template v-if="item.status === 'finished'">
              <img :src="item.url" />
              <div class="demo-upload-list-cover">
                <Icon
                  type="ios-eye-outline"
                  style="float: left;"
                  @click.native="handleView(item.url)"
                ></Icon>
                <Icon
                  type="ios-trash-outline"
                  style="float: right;"
                  @click.native="handleRemove(item.name)"
                ></Icon>
              </div>
            </template>
            <template v-else>
              <Progress v-if="item.showProgress" :percent="item.percentage" hide-info></Progress>
            </template>
          </div>
          <Divider dashed />
          <Upload
            ref="upload"
            :show-upload-list="false"
            :default-file-list="defaultList"
            :on-success="showUpResult"
            :on-progress="toUpResult"
            :format="['jpg','jpeg','png']"
            :max-size="5120"
            :data="{uuid:this.$store.state.user.userGuid}"
            :on-format-error="handleFormatError"
            :on-exceeded-size="handleMaxSize"
            :before-upload="handleBeforeUpload"
            type="drag"
            :action="actionurl"
            :headers="postheaders"
            style="display: inline-block;width:58px;"
          >
            <div style="width: 58px;height:58px;line-height: 58px;">
              <Icon type="ios-camera" size="20"></Icon>
            </div>
          </Upload>
          <Modal title="查看图片" v-model="visible">
            <img :src="imgName" v-if="visible" style="width: 100%" />
          </Modal>
        </Row>
        <Row v-if="checkShow()">
          <div class="demo-upload-list" v-for="item in uploadList">
            <template v-if="item.status === 'finished'">
              <img :src="item.url" />
              <div class="demo-upload-list-cover">
                <Icon type="ios-eye-outline" @click.native="handleView(item.url)"></Icon>
              </div>
            </template>
            <template v-else>
              <Progress v-if="item.showProgress" :percent="item.percentage" hide-info></Progress>
            </template>
          </div>
        </Row>
      </Form>
      <div class="demo-drawer-footer" v-if="!checkShow()">
        <Button icon="md-checkmark-circle" type="primary" @click="handleSubmitIngredient">保 存</Button>
        <Button style="margin-left: 8px" icon="md-close" @click="formModel.opened = false">取 消</Button>
      </div>
    </Drawer>
  </div>
</template>

<script>
import DzTable from "_c/tables/dz-table.vue";
import {
  getIngredientList,
  createIngredient,
  loadIngredient,
  editIngredient,
  deleteIngredient,
  batchCommand,
  deletetoFile,
  GetFoodTypeList
} from "@/api/ingredient/ingredient";
import config from "@/config";
import { getToken } from "@/libs/util";
import {
  globalvalidateIDCardNoMust,
  globalvalidatepwd,
  globalvalidateIsNotEmpty,
  globalvalidateNumIsNotEmpty
} from "@/global/validate";
export default {
  name: "ingredient",
  components: {
    DzTable
  },
  data() {
    return {
      url: config.baseUrl.dev,
      imgName: "",
      visible: false,
      updisabled: false,
      uploadList: [],
      defaultList: [],
      loadingStatus: false,
      actionurl: "",
      postheaders: "",
      commands: {
        delete: { name: "delete", title: "删除" },
        recover: { name: "recover", title: "恢复" }
        // forbidden: { name: "forbidden", title: "禁用" },
        // normal: { name: "normal", title: "启用" }
      },
      schoolName:"",
      formModel: {
        opened: false,
        title: "创建用户",
        mode: "create",
        selection: [],
        fields: {
          ingredientUuid: "",
          foodName: "",
          typeUuid: "",
          heatEnergy: 0,
          protein: 0,
          fat: 0,
          saccharides: 0,
          va: 0,
          addTime: "",
          addPeople: "",
          isDelete: "",
          schoolUuid: "",
          accessory: "",
          schoolName:"",
        },
        rules: {
          foodName: [
            {
              validator: globalvalidateIsNotEmpty,
              type: "string",
              required: true,
              message: "请输入食材名称"
              ,trigger:'blur'
            }
          ],
          typeUuid: [{validator: globalvalidateIsNotEmpty, required: true, message: "请选择类型",trigger:'blur' }],
          heatEnergy:[{validator: globalvalidateNumIsNotEmpty,type:"number",message:"请输入数字"}],
          protein:[{validator: globalvalidateNumIsNotEmpty,type:"number",message:"请输入数字"}],
          fat:[{validator: globalvalidateNumIsNotEmpty,type:"number",message:"请输入数字"}],
          saccharides:[{validator: globalvalidateNumIsNotEmpty,type:"number",message:"请输入数字"}],
          va:[{validator: globalvalidateNumIsNotEmpty,type:"number",message:"请输入数字"}],

        }
      },
      formAssignRole: {
        userGuid: "",
        opened: false,
        ownedRoles: [],
        inited: false,
        roles: []
      },
      stores: {
        ingredient: {
          query: {
            totalCount: 0,
            pageSize: 20,
            currentPage: 1,
            kw: "",
            kw2: "",
            isDeleted: 0,
            status: -1,
            sort: [
              {
                direct: "DESC",
                field: "ID"
              }
            ]
          },
          sources: {
            types: [


//               乳与乳制品
// 脂肪、油和乳化脂肪制品
// 冷冻饮品
// 水果、蔬菜(包括块根类)、豆类、食用菌、藻类、坚果以及籽类等
// 可可制品、巧克力和巧克力制品(包括类巧克力和代巧克力)以及糖果
// 粮食和粮食制品
// 焙烤食品
// 肉及肉制品
// 水产品及其制品
// 蛋及蛋制品
// 甜味料
// 调味品
// 特殊营养食品
// 饮料类
// 酒类
// 其他类

            ],
            types2: [
            ],
            isDeletedSources: [
              { value: -1, text: "全部" },
              { value: 0, text: "正常" },
              { value: 1, text: "已删" }
            ],
            statusSources: [
              { value: -1, text: "全部" },
              { value: 0, text: "禁用" },
              { value: 1, text: "正常" }
            ],
            statusFormSources: [
              { value: 0, text: "禁用" },
              { value: 1, text: "正常" }
            ]
          },
          columns: [
            { type: "selection", width: 50, key: "handle" },
            { title: "食材名称", key: "foodName", sortable: true },
            { title: "类别", key: "type" },
            { title: "热能(Kcal/100g)", key: "heatEnergy" },
            { title: "蛋白质(g/100g)", key: "protein" },
            { title: "脂肪(g/100g)", key: "fat" },
            { title: "糖类(g/100g)", key: "saccharides" },
            { title: "VA(ug/100g)", key: "va" },
            {
              title: "操作",
              align: "center",
              width: 150,
              className: "table-command-column",
              slot: "action"
            }
          ],
          data: []
        }
      },
      styles: {
        height: "calc(100% - 55px)",
        overflow: "auto",
        paddingBottom: "53px",
        position: "static"
      },
      rolelist: [],
      departmentlist: [],
      schoolList: []
    };
  },
  computed: {
    formTitle() {
      if (this.formModel.mode === "create") {
        return "创建食材";
      }
      if (this.formModel.mode === "edit") {
        return "编辑食材";
      }
      if(this.formModel.mode==="show"){
        let s="食材详情";
        if(this.formModel.fields.schoolName!=""&&this.$store.state.user.schoolguid==null){
          s+="   学校:("+this.formModel.fields.schoolName+")";
        }
        
        return s;
      }
      return "";
    },
    selectedRows() {
      return this.formModel.selection;
    },
    selectedRowsId() {
      return this.formModel.selection.map(x => x.ingredientUuid);
    }
  },
  methods: {
    loadIngredientList() {
      getIngredientList(this.stores.ingredient.query).then(res => {
        this.stores.ingredient.data = res.data.data;
        this.stores.ingredient.query.totalCount = res.data.totalCount;
      });
    },
    doGetFoodTypeList(){
      GetFoodTypeList().then(res=>{
        console.log(res);
        this.stores.ingredient.sources.types=res.data.data;
        this.stores.ingredient.sources.types2=res.data.data;
      })
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
      this.handleOpenFormWindow();
    },
    handleSwitchFormModeToEdit() {
      this.formModel.mode = "edit";
      this.handleOpenFormWindow();
    },
    handleSwitchFormModeToShow() {
      this.formModel.mode = "show";
      this.handleOpenFormWindow();
    },
    handleEdit(row) {
      this.uploadList = [];
      this.formModel.fields.accessory = "";
      this.handleSwitchFormModeToEdit();
      this.handleResetFormIngredient();
      this.doLoadIngredient(row.ingredientUuid);
    },
    handleShow(row) {
      this.uploadList = [];
      this.formModel.fields.accessory = "";
      this.handleSwitchFormModeToShow();
      this.handleResetFormIngredient();
      this.doLoadIngredient(row.ingredientUuid);
    },
    handleSelect(selection, row) {},
    handleSelectionChange(selection) {
      this.formModel.selection = selection;
    },
    handleRefresh() {
      this.loadIngredientList();
    },
    handleShowCreateWindow() {
      this.uploadList = [];
      this.formModel.fields.accessory = "";
      this.handleSwitchFormModeToCreate();
      this.handleResetFormIngredient();
    },
    handleSubmitIngredient() {
      // if (
      //   this.formModel.fields.accessory == null ||
      //   this.formModel.fields.accessory == ""
      // ) {
      //   this.$Message.warning("请上传图片");
      //   return;
      // }
      let valid = this.validateIngredientForm();
      //console.log(valid);
      //console.log(this.formModel.fields);
      if (valid) {
        if (this.formModel.mode === "create") {
          this.doCreateIngredient();
        }
        if (this.formModel.mode === "edit") {
          this.doEditIngredient();
        }
      }
    },
    handleResetFormIngredient() {
      this.$refs["formIngredient"].resetFields();
    },
    doCreateIngredient() {
      this.formModel.fields.addPeople = this.$store.state.user.userName;
      createIngredient(this.formModel.fields).then(res => {
        if (res.data.code === 200) {
          this.$Message.success(res.data.message);
          this.handleCloseFormWindow();
          this.loadIngredientList();
        } else {
          this.$Message.warning(res.data.message);
        }
      });
    },
    doEditIngredient() {
      this.formModel.fields.addPeople = this.$store.state.user.userName;
      editIngredient(this.formModel.fields).then(res => {
        if (res.data.code === 200) {
          this.$Message.success(res.data.message);
          this.handleCloseFormWindow();
          this.loadIngredientList();
        } else {
          this.$Message.warning(res.data.message);
        }
      });
    },
    validateIngredientForm() {
      let _valid = false;
      this.$refs["formIngredient"].validate(valid => {
        if (!valid) {
          this.$Message.error("请完善表单信息");
        } else {
          _valid = true;
        }
      });
      return _valid;
    },
    doLoadIngredient(uuid) {
      loadIngredient({ guid: uuid }).then(res => {
        // console.log(res.data.data);
        this.formModel.fields = res.data.data;
        console.log(this.formModel.fields.accessory);
        if (this.formModel.fields.accessory != null) {
          let list = this.formModel.fields.accessory.split(",");
          this.uploadList = [];
          for (let i = 0; i < list.length; i++) {
            this.uploadList.push({
              url:
                config.baseUrl.dev + "UploadFiles/LiveShotPicture/" + list[i],
              status: "finished",
              name: "UploadFiles/LiveShotPicture/" + list[i],
              fileName: list[i]
            });
          }
        }else{
this.uploadList = [];
        }
        
      });
    },
    handleDelete(row) {
      this.doDelete(row.ingredientUuid);
    },
    doDelete(ids) {
      if (!ids) {
        this.$Message.warning("请选择至少一条数据");
        return;
      }
      deleteIngredient(ids).then(res => {
        if (res.data.code === 200) {
          this.$Message.success(res.data.message);
          this.loadIngredientList();
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
        }
      });
    },
    doBatchCommand(command) {
      batchCommand({
        command: command,
        ids: this.selectedRowsId.join(",")
      }).then(res => {
        if (res.data.code === 200) {
          this.$Message.success(res.data.message);
          this.loadIngredientList();
          this.formModel.selection = [];
        } else {
          this.$Message.warning(res.data.message);
        }
        this.$Modal.remove();
      });
    },
    handleSearchIngredient() {
      this.loadIngredientList();
    },
    rowClsRender(row, index) {
      if (row.isDeleted) {
        return "table-row-disabled";
      }
      return "";
    },
    handleSortChange(column) {
      this.stores.ingredient.query.sort.direction = column.order;
      this.stores.ingredient.query.sort.field = column.key;
      this.loadIngredientList();
    },
    handlePageChanged(page) {
      this.stores.ingredient.query.currentPage = page;
      this.loadIngredientList();
    },
    handlePageSizeChanged(pageSize) {
      this.stores.ingredient.query.pageSize = pageSize;
      this.loadIngredientList();
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
        text: "正常"
      };
      switch (status) {
        case 0:
          _status.text = "禁用";
          _status.color = "error";
          break;
      }
      return _status;
    },
    checkShow() {
      return this.formModel.mode === "show";
    },
    async doLoadLiveShot(uuid) {
      await loadLiveShot({ guid: uuid }).then(res => {
        console.log(res.data.data);
        if (this.formModel.mode != "create") {
          console.log(res.data.data.entity.datetime);
          this.loadCuiSineList2(
            res.data.data.entity.datetime,
            res.data.data.entity.datetype
          );
        }
        this.formModel.fields = res.data.data.entity;
        console.log(1111);
        console.log(this.formModel.fields.accessory);
        let list = this.formModel.fields.accessory.split(",");
        this.uploadList = [];
        for (let i = 0; i < list.length; i++) {
          this.uploadList.push({
            url: config.baseUrl.dev + "UploadFiles/LiveShotPicture/" + list[i],
            status: "finished",
            name: "UploadFiles/LiveShotPicture/" + list[i],
            fileName: list[i]
          });
        }
        // this.$refs.upload.fileList[0].name = this.formModel.fields.accessory.split(
        //   "\\"
        // )[1];
        // this.formModel.dFileName = res.data.data.path;
        // this.url =
        //   config.baseUrl.dev +
        //   this.formModel.fields.accessory.replace("\\", "/");
        //this.formModel.selectPeople = res.data.data.systemUserUuid.split(",");
        // console.log(this.defaultfilelist);
      });
    },
    async showUpResult(response, file, fileList) {
      console.log(this.$refs.upload.fileList);
      console.log(1);
      console.log(response);
      console.log(file);
      console.log(fileList);
      this.loadingStatus = false;
      this.updisabled = false;
      if (response.code == "200") {
        this.$Message.success(response.message);
        console.log(this.formModel.fields.accessory);
        if (this.formModel.fields.accessory != null) {
          if (this.formModel.fields.accessory.length > 0) {
            this.formModel.fields.accessory += ",";
          }
          this.formModel.fields.accessory += response.data.fname;
        } else {
          this.formModel.fields.accessory = response.data.fname;
        }
        // this.formModel.dFileName = response.data.paths;

        await this.uploadList.push({
          url: config.baseUrl.dev + response.data.strpath.replace("\\", "/"),
          status: "finished",
          name: response.data.strpath,
          fileName: response.data.fname
        });

        // console.log(
        //   (this.$refs.upload.fileList[0].name = e.data.dataPath.split("\\")[1])
        // );
        // console.log(this.defaultfilelist);
        // if (this.departmentlist.length >= 1) {
        //   this.defaultfilelist = [
        //     { name: this.formModel.fields.name, url: e.data.path }
        //   ];
        // }
        console.log(this.formModel.dFileName);
      } else {
        this.$Message.warning(response.message);
      }
    },
    toUpResult() {
      console.log(this.$refs.upload.fileList);
      //console.log(this.$refs.upload.fileList);
      if (this.$refs.upload.fileList.length > 1) {
        this.$refs.upload.fileList.shift();
        // this.$refs.upload.clearFiles();
        // this.$refs.upload.push({})
      }
      this.loadingStatus = true;
      this.updisabled = true;
    },
    deleteFile(e) {
      console.log(e);
      console.log(this.formModel.dFileName);
      // if (this.formModel.dFileName != null && this.formModel.dFileName != "") {
      //   deletetoFile({ filename: this.formModel.dFileName }).then(res => {
      //     console.log(res);
      //   });
      // }
    },
    download() {
      // console.log(this.url);
      window.location.href =
        config.baseUrl.dev + this.formModel.fields.accessory.replace("\\", "/");
    },
    handleView(name) {
      this.imgName = name;
      this.visible = true;
    },
    handleRemove(file) {
      console.log(file);
      // const fileList = this.$refs.upload.fileList;
      // this.$refs.upload.fileList.splice(fileList.indexOf(file), 1);
      deletetoFile({ path: file }).then(res => {
        console.log(res);
        if (res.data.data == "200") {
          this.uploadList = this.uploadList.filter(x => x.name != file);
          this.formModel.fields.accessory = this.uploadList
            .map(x => x.fileName)
            .join(",");
        } else {
          this.uploadList = this.uploadList.filter(x => x.name != file);
          this.formModel.fields.accessory = this.uploadList
            .map(x => x.fileName)
            .join(",");
        }
      });
    },

    handleFormatError(file) {
      this.$Notice.warning({
        title: "The file format is incorrect",
        desc: "文件: " + file.name + " 不是png,jpg"
      });
    },
    handleMaxSize(file) {
      this.$Notice.warning({
        title: "Exceeding file size limit",
        desc: "文件 " + file.name + " 太大,超过5M."
      });
    },
    handleBeforeUpload() {
      // const check = this.uploadList.length < 5;
      // if (!check) {
      //   this.$Notice.warning({
      //     title: "Up to five pictures can be uploaded."
      //   });
      // }
      // return check;
      return true;
    },
    checkRead(){
      if(this.formModel.mode=="show"){
        return true;
      }else{
        return false;
      }
    }
  },
  mounted() {
    this.doGetFoodTypeList();
    console.log(this.$store.state.user);
    this.loadIngredientList();
  },
  created() {
    if(this.$store.state.user.schoolguid==null){
      this.stores.ingredient.columns.splice(1,0,{title: "学校", key: "schoolName"})
    }
    this.postheaders = {
      Authorization: "Bearer " + getToken()
      //Accept: "application/json, text/plain, */*"
    };
    this.actionurl = config.baseUrl.dev + "api/v1/ingredient/ingredient/UpLoad";
  }
};
</script>

<style>
.inputnum{
  width: 300px;
}
.demo-upload-list {
  display: inline-block;
  width: 120px;
  height: 120px;
  text-align: center;
  line-height: 120px;
  border: 1px solid transparent;
  border-radius: 4px;
  overflow: hidden;
  background: #fff;
  position: relative;
  box-shadow: 0 1px 1px rgba(0, 0, 0, 0.2);
  margin-right: 4px;
}
.demo-upload-list img {
  width: 100%;
  height: 100%;
}
.demo-upload-list-cover {
  display: none;
  position: absolute;
  top: 0;
  bottom: 0;
  left: 0;
  right: 0;
  background: rgba(0, 0, 0, 0.6);
}
.demo-upload-list:hover .demo-upload-list-cover {
  display: block;
}
.demo-upload-list-cover i {
  color: #fff;
  font-size: 20px;
  cursor: pointer;
  margin: 0 2px;
}
</style>
